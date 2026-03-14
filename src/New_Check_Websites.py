import os
import time
import requests
import sys
from pymongo import MongoClient
from datadog import initialize, api
from dotenv import load_dotenv

# --- 1. CONFIGURAZIONE ---
load_dotenv()

# Recupero variabili dai Secrets di GitHub
MONGODB_URI = os.getenv('MONGODB_URI')
DD_API_KEY = os.getenv('DD_API_KEY')
DD_SITE = os.getenv('DD_SITE', 'us5.datadoghq.com')

# Controllo sicurezza
if not MONGODB_URI or not DD_API_KEY:
    print("❌ ERRORE: Variabili MONGODB_URI o DD_API_KEY mancanti nei Secrets!")
    sys.exit(1)

# --- 2. INIZIALIZZAZIONE DATADOG ---
options = {
    'api_key': DD_API_KEY,
    'api_host': f"https://api.{DD_SITE}"
}
initialize(**options)

# --- 3. CONNESSIONE MONGODB (Versione robusta per il Cloud) ---
try:
    # Aggiungiamo timeout e dns per evitare l'errore 'DNS query name does not exist'
    client = MongoClient(
        MONGODB_URI,
        connectTimeoutMS=30000,
        socketTimeoutMS=30000,
        serverSelectionTimeoutMS=30000
    )
    db = client['TechTutorPlay_Lab']
    collection = db['site_monitoring_logs']
    # Test della connessione
    client.admin.command('ping')
    print(f"✅ Connesso con successo a MongoDB Cluster")
except Exception as e:
    print(f"❌ Errore critico connessione MongoDB: {e}")
    sys.exit(1)

# --- 4. LOGICA DI MONITORAGGIO ---
def monitora(url):
    try:
        start = time.time()
        r = requests.get(url, timeout=15)
        latenza = time.time() - start
        status = "ONLINE" if r.status_code == 200 else "OFFLINE"
        
        # Invio a Datadog
        tags = [f"site:{url}", "env:cloud_github"]
        api.Metric.send(metric='techtutorplay.latency', points=[(int(time.time()), latenza)], tags=tags)
        
        # Salvataggio su MongoDB
        collection.insert_one({
            "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
            "url": url,
            "latency": round(latenza, 4),
            "status": status,
            "provider": "GitHub_Actions"
        })
        print(f"🚀 {url} -> {status} ({latenza:.3f}s)")
    except Exception as e:
        print(f"⚠️ Errore su {url}: {e}")

if __name__ == "__main__":
    siti = ["https://www.techtutorplay.tech", "https://www.techtutorplay.com"]
    print(f"--- Inizio Check: {time.strftime('%H:%M:%S')} ---")
    for s in siti:
        monitora(s)
    print("--- Fine Check ---")
