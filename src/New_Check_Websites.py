import os
import time
import requests
import sys
from pymongo import MongoClient
from datadog import initialize, api # Importiamo 'api' per l'invio diretto senza Agent
from dotenv import load_dotenv

# --- 1. CARICAMENTO CONFIGURAZIONE ---
# Carica il file .env solo se esiste (utile per i test locali sul tuo PC)
load_dotenv()

# Recupero delle variabili d'ambiente (da GitHub Secrets o file .env)
# Manteniamo il collegamento a MongoDB (Cluster Belgio)
MONGODB_URI = os.getenv('MONGODB_URI')
DD_API_KEY = os.getenv('DD_API_KEY')
# Usiamo US5 come sito Datadog
DD_SITE = os.getenv('DD_SITE', 'us5.datadoghq.com')

# Controllo sicurezza: se mancano le chiavi, lo script si ferma
if not MONGODB_URI or not DD_API_KEY:
    print("❌ ERRORE: Variabili MONGODB_URI o DD_API_KEY mancanti!")
    sys.exit(1)

# --- 2. INIZIALIZZAZIONE DATADOG (API MODE) ---
options = {
    'api_key': DD_API_KEY,
    'api_host': f"https://api.{DD_SITE}"
}
initialize(**options)

# --- 3. CONNESSIONE MONGODB ---
try:
    client = MongoClient(MONGODB_URI)
    db = client['TechTutorPlay_Lab']
    collection = db['site_monitoring_logs']
    print(f"✅ Connesso a MongoDB: {db.name}")
except Exception as e:
    print(f"❌ Errore connessione MongoDB: {e}")
    sys.exit(1)

# --- 4. FUNZIONE CORE DI MONITORAGGIO ---
def esegui_check(url):
    try:
        # Calcolo della latenza (tempo di risposta)
        inizio = time.time()
        risposta = requests.get(url, timeout=15)
        latenza = time.time() - inizio
        
        status = "ONLINE" if risposta.status_code == 200 else "OFFLINE"
        timestamp_attuale = int(time.time())

        # --- INVIO A DATADOG VIA API ---
        # Creiamo i tag per filtrare i dati nella Dashboard
        tag_list = [f"site:{url}", "env:cloud_github", "version:1.0"]
        
        # Invio della metrica di latenza
        api.Metric.send(
            metric='techtutorplay.latency',
            points=[(timestamp_attuale, latenza)],
            tags=tag_list
        )
        
        # Invio di un contatore per lo stato
        api.Metric.send(
            metric='techtutorplay.status_check',
            points=[(timestamp_attuale, 1)],
            tags=tag_list + [f"status:{status}"]
        )

        # --- SALVATAGGIO SU MONGODB ---
        log_data = {
            "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
            "url": url,
            "latency": round(latenza, 4),
            "status": status,
            "http_code": risposta.status_code,
            "provider": "GitHub_Actions_Cloud"
        }
        collection.insert_one(log_data)
        
        print(f"🚀 [OK] {url} | Latenza: {latenza:.3f}s | Status: {status}")

    except Exception as e:
        print(f"⚠️ [ERRORE] Impossibile raggiungere {url}: {e}")
        # Notifica l'errore a Datadog
        api.Metric.send(
            metric='techtutorplay.errors',
            points=[(int(time.time()), 1)],
            tags=[f"site:{url}", "type:connection_error"]
        )

# --- 5. ESECUZIONE ---
if __name__ == "__main__":
    siti_da_monitorare = [
        "https://www.techtutorplay.tech", 
        "https://www.techtutorplay.com"
    ]
    
    print(f"\n--- AVVIO MONITORAGGIO CLOUD: {time.strftime('%H:%M:%S')} ---")
    for sito in siti_da_monitorare:
        esegui_check(sito)
    print("--- MONITORAGGIO COMPLETATO ---\n")
