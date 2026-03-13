import os
import requests
import pymongo
from datetime import datetime
from dotenv import load_dotenv

# --- CONFIGURAZIONE PERCORSI ---
# Troviamo la cartella dove si trova questo file .py
cartella_script = os.path.dirname(os.path.abspath(__file__))

# Indichiamo a Python che il file si chiama 'Secrets.env'
percorso_env = os.path.join(cartella_script, 'Secrets.env')

# Carichiamo il file specifico
load_dotenv(percorso_env)

def avvia_sistema():
    print("=== TECHTUTORPLAY LAB: MONITORING SYSTEM ===")
    
    # 1. DEBUG: Verifichiamo se il file Secrets.env esiste davvero
    if not os.path.exists(percorso_env):
        print(f"[ERRORE] File '{percorso_env}' NON TROVATO!")
        print("Controlla che il file sia nella stessa cartella dello script.")
        return

    # 2. Caricamento variabili dal file Secrets.env
    uri_mongo = os.getenv("MONGODB_URI")
    url_com = os.getenv("URL_COM")
    url_tech = os.getenv("URL_TECH")
    
    # Debug rapido per te (non preoccuparti, non stampa la password intera)
    print(f"[INFO] Caricamento variabili: {'OK' if uri_mongo else 'FALLITO'}")
    
    if not uri_mongo:
        print("[ERRORE] MONGODB_URI non trovata dentro Secrets.env!")
        return

    # 3. Connessione a MongoDB Atlas
    try:
        # Usiamo un timeout di 5 secondi per non restare appesi
        client = pymongo.MongoClient(uri_mongo, serverSelectionTimeoutMS=5000)
        # Test rapido di connessione
        client.admin.command('ping')
        
        db = client.TechTutorPlay_Lab
        collezione = db.site_monitoring_logs
        print("[SUCCESS] Connessione a MongoDB Atlas STABILITA.\n")
        
    except Exception as e:
        print(f"[ERRORE] Connessione MongoDB fallita: {e}")
        print("Suggerimento: Controlla se il tuo IP è abilitato su Network Access in Atlas.")
        return

    # 4. Monitoraggio Siti
    siti = [url_com, url_tech]
    for url in siti:
        if not url: continue
        
        print(f"Controllo in corso: {url}...")
        try:
            inizio = datetime.now()
            r = requests.get(url, timeout=10)
            latenza = (datetime.now() - inizio).total_seconds()
            stato = "ONLINE" if r.status_code == 200 else f"HTTP {r.status_code}"
        except:
            stato = "OFFLINE"
            latenza = 0

        # Creazione del pacchetto dati (Documento)
        documento = {
            "sito": url,
            "stato": stato,
            "latenza_sec": latenza,
            "data_ora": datetime.now(),
            "hardware": "RTX-5070-Lab"
        }

        # Invio a MongoDB
        try:
            collezione.insert_one(documento)
            print(f" >> [{stato}] Risposta in {latenza:.3f}s - Dati inviati al Cloud.")
        except Exception as e:
            print(f" >> [!] Errore salvataggio: {e}")

    client.close()
    print("\n=== Monitoraggio completato con successo ===")

if __name__ == "__main__":
    avvia_sistema()

input("Premi INVIO per chiudere...") # Mantiene la finestra aperta dopo l'esecuzione, così puoi vedere i risultati.
