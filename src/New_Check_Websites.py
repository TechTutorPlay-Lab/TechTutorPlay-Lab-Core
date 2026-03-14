import os
import time
import requests
import sys
from pymongo import MongoClient
from datadog import initialize, api

# --- 1. CONFIGURAZIONE ---
# Recuperiamo le chiavi dai Secrets di GitHub Actions
MONGODB_URI = os.getenv('MONGODB_URI')
DD_API_KEY = os.getenv('DD_API_KEY')
DD_SITE = "us5.datadoghq.com" # Il tuo datacenter Datadog

# Inizializzazione API Datadog
options = {
    'api_key': DD_API_KEY,
    'api_host': f"https://api.{DD_SITE}"
}
initialize(**options)

# --- 2. CONNESSIONE MONGODB ---
try:
    # Connessione al Cluster con bypass SSL per GitHub Actions
    client = MongoClient(
        MONGODB_URI,
        tls=True,
        tlsAllowInvalidCertificates=True,
        serverSelectionTimeoutMS=5000
    )
    # Puntiamo al tuo database principale
    db = client['TechTutorPlay-Main']
    collection = db['site_monitoring_logs']
    
    # Test della connessione
    client.admin.command('ping')
    print("✅ MongoDB: Connesso a TechTutorPlay-Main")
except Exception as e:
    print(f"❌ MongoDB Errore: {e}")
    sys.exit(1)

# --- 3. FUNZIONE DI MONITORAGGIO ---
def esegui_check(url):
    try:
        # Calcolo latenza
        inizio = time.time()
        r = requests.get(url, timeout=15)
        latenza = time.time() - inizio
        status = "ONLINE" if r.status_code == 200 else "OFFLINE"

        # --- INVIO A DATADOG (DEBUG) ---
        # Inviamo la metrica con il tag specifico per il sito
        dd_res = api.Metric.send(
            metric='techtutorplay.latency',
            points=[(int(time.time()), latenza)],
            tags=[f"site:{url}", "env:cloud_github"]
        )
        print(f"📊 Datadog ({url}): {dd_res}")

        # --- SALVATAGGIO SU MONGODB ---
        collection.insert_one({
            "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
            "url": url,
            "latency": round(latenza, 4),
            "status": status,
            "provider": "GitHub_Actions_Cloud"
        })
        print(f"🚀 MongoDB: Salvato check per {url}")

    except Exception as e:
        print(f"⚠️ Errore durante il check di {url}: {e}")

# --- 4. ESECUZIONE ---
if __name__ == "__main__":
    # La tua flotta di domini da controllare
    siti = [
        "https://www.techtutorplay.tech", 
        "https://www.techtutorplay.com"
    ]
    
    print(f"--- Inizio Monitoraggio: {time.strftime('%H:%M:%S')} ---")
    for s in siti:
        esegui_check(s)
    print("--- Fine Monitoraggio ---")
