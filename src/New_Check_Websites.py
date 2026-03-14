import os
import time
import requests
import sys
from pymongo import MongoClient
from datadog import initialize, api

# --- 1. CONFIGURAZIONE E VARIABILI ---
# Recuperiamo i Secrets impostati su GitHub
MONGODB_URI = os.getenv('MONGODB_URI')
DD_API_KEY = os.getenv('DD_API_KEY')
DD_SITE = os.getenv('DD_SITE', 'us5.datadoghq.com')

# Controllo che le chiavi siano caricate
if not MONGODB_URI or not DD_API_KEY:
    print("❌ ERRORE: MONGODB_URI o DD_API_KEY non trovati nei Secrets!")
    sys.exit(1)

# --- 2. INIZIALIZZAZIONE DATADOG (Cloud Mode) ---
options = {
    'api_key': DD_API_KEY,
    'api_host': f"https://api.{DD_SITE}"
}
initialize(**options)

# --- 3. CONNESSIONE MONGODB (Driver 3.12 Style) ---
try:
    # Usiamo parametri robusti per evitare timeout e blocchi SSL nel cloud
    client = MongoClient(
        MONGODB_URI,
        connectTimeoutMS=30000,
        socketTimeoutMS=30000,
        tls=True,
        tlsAllowInvalidCertificates=True  # Risolve l'errore SSL Handshake Failed
    )
    db = client['TechTutorPlay_Lab']
    collection = db['site_monitoring_logs']
    
    # Test della connessione (Ping)
    client.admin.command('ping')
    print("✅ CONNESSIONE RIUSCITA: MongoDB è pronto.")
except Exception as e:
    print(f"❌ ERRORE CONNESSIONE MONGODB: {e}")
    sys.exit(1)

# --- 4. FUNZIONE DI MONITORAGGIO ---
def monitora(url):
    try:
        start_time = time.time()
        # Facciamo la richiesta al sito
        r = requests.get(url, timeout=15)
        latenza = time.time() - start_time
        status = "ONLINE" if r.status_code == 200 else "OFFLINE"
        
        # A. INVIO METRICHE A DATADOG
        timestamp = int(time.time())
        tags = [f"site:{url}", "env:cloud_github", "version:3.12_driver"]
        
        api.Metric.send(
            metric='techtutorplay.latency',
            points=[(timestamp, latenza)],
            tags=tags
        )
        
        # B. SALVATAGGIO LOG SU MONGODB
        log_data = {
            "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
            "url": url,
            "latency": round(latenza, 4),
            "status": status,
            "http_code": r.status_code,
            "provider": "GitHub_Actions_Cloud"
        }
        collection.insert_one(log_data)
        
        print(f"🚀 {url} -> {status} | Latenza: {latenza:.3f}s")

    except Exception as e:
        print(f"⚠️ ERRORE SU {url}: {e}")
        # Segnaliamo l'errore a Datadog
        api.Metric.send(
            metric='techtutorplay.errors',
            points=[(int(time.time()), 1)],
            tags=[f"site:{url}", "error:timeout"]
        )

# --- 5. ESECUZIONE ---
if __name__ == "__main__":
    siti_lab = [
        "https://www.techtutorplay.tech", 
        "https://www.techtutorplay.com"
    ]
    
    print(f"\n--- AVVIO CHECK CLOUD: {time.strftime('%Y-%m-%d %H:%M:%S')} ---")
    for sito in siti_lab:
        monitora(sito)
    print("--- MONITORAGGIO COMPLETATO ---\n")
