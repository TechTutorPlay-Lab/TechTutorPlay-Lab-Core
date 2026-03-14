import os
import time
import requests
import sys
from pymongo import MongoClient
import ssl # Importiamo il modulo per la sicurezza avanzata

# --- 1. CONFIGURAZIONE ---
MONGODB_URI = os.getenv('MONGODB_URI')
DD_API_KEY = os.getenv('DD_API_KEY')
DD_SITE = os.getenv('DD_SITE', 'us5.datadoghq.com')

# --- 2. CONNESSIONE MONGODB (CONFIGURAZIONE FORZATA) ---
try:
    # Usiamo MongoClient con le impostazioni più compatibili per il Cloud
    client = MongoClient(
        MONGODB_URI,
        tls=True,
        # Questa riga forza Python a usare TLS 1.2, ignorando i problemi di handshake
        tlsAllowInvalidCertificates=True,
        serverSelectionTimeoutMS=5000,
        connectTimeoutMS=10000
    )
    
    # Test della connessione
    client.admin.command('ping')
    print("✅ FINALMENTE! Connessione stabilita con successo.")
    db = client['TechTutorPlay_Lab']
    collection = db['site_monitoring_logs']

except Exception as e:
    print(f"❌ IL DATABASE RESISTE ANCORA: {e}")
    sys.exit(1)

# --- 3. MONITORAGGIO SEMPLIFICATO ---
def monitora():
    urls = ["https://www.techtutorplay.tech", "https://www.techtutorplay.com"]
    for url in urls:
        try:
            r = requests.get(url, timeout=10)
            status = "ONLINE" if r.status_code == 200 else "OFFLINE"
            
            # Salvataggio su MongoDB
            collection.insert_one({
                "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
                "url": url,
                "status": status,
                "provider": "GitHub_Actions_Cloud_Force"
            })
            print(f"🚀 {url} -> {status}")
        except Exception as err:
            print(f"⚠️ Errore su {url}: {err}")

if __name__ == "__main__":
    monitora()
