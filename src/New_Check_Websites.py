import os
import time
import requests
import sys
from pymongo import MongoClient
from datadog import initialize, api

# --- 1. CONFIGURAZIONE ---
MONGODB_URI = os.getenv('MONGODB_URI')
DD_API_KEY = os.getenv('DD_API_KEY')
DD_SITE = os.getenv('DD_SITE', 'us5.datadoghq.com')

# Inizializza Datadog
initialize(api_key=DD_API_KEY, api_host=f"https://api.{DD_SITE}")

# --- 2. CONNESSIONE MONGODB (MODALITÀ COMPATIBILITÀ CLOUD) ---
try:
    # Usiamo la configurazione più permissiva possibile per bypassare i blocchi SSL di GitHub
    client = MongoClient(
        MONGODB_URI,
        tls=True,
        tlsAllowInvalidCertificates=True,
        tlsInsecure=True, # Forza la connessione anche se l'handshake fallisce
        serverSelectionTimeoutMS=10000,
        connectTimeoutMS=10000
    )
    
    # Test della connessione
    client.admin.command('ping')
    print("✅ CONNESSIONE RIUSCITA: MongoDB Atlas ha accettato il Lab!")
    db = client['TechTutorPlay_Lab']
    collection = db['site_monitoring_logs']

except Exception as e:
    print(f"❌ ERRORE CRITICO CONNESSIONE: {e}")
    sys.exit(1)

# --- 3. MONITORAGGIO ---
def monitora(url):
    try:
        start = time.time()
        r = requests.get(url, timeout=15)
        latenza = time.time() - start
        status = "ONLINE" if r.status_code == 200 else "OFFLINE"
        
        # Invio a Datadog
        api.Metric.send(
            metric='techtutorplay.latency',
            points=[(int(time.time()), latenza)],
            tags=[f"site:{url}", "env:cloud_github"]
        )
        
        # Salvataggio su MongoDB
        collection.insert_one({
            "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
            "url": url,
            "latency": round(latenza, 4),
            "status": status,
            "provider": "GitHub_Actions_Final_Fix"
        })
        print(f"🚀 {url} -> {status} ({latenza:.3f}s)")

    except Exception as e:
        print(f"⚠️ Errore su {url}: {e}")

if __name__ == "__main__":
    siti = ["https://www.techtutorplay.tech", "https://www.techtutorplay.com"]
    print(f"--- Avvio monitoraggio: {time.strftime('%H:%M:%S')} ---")
    for s in siti:
        monitora(s)
    print("--- Completato ---")
