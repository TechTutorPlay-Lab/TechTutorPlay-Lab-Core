import os        # Aggiungi questa riga all'inizio (serve per leggere il sistema)
import requests
import json

# --- CONFIGURAZIONE SICURA ---
# Chiediamo a Windows di darci il token che abbiamo salvato prima
TOKEN = os.getenv("GITHUB_LAB_TOKEN") 

# Controllo di sicurezza: se il token non c'è, lo script si ferma invece di crashare
if not TOKEN:
    print("❌ Errore: Non ho trovato la variabile 'GITHUB_LAB_TOKEN' su Windows!")
    print("Assicurati di averla creata nelle Variabili d'Ambiente e riavviato il terminale/VS Code.")
    exit()
# Il tuo username e il nome della repository del TechTutorPlay Lab
REPO_OWNER = "TechTutorPlay-Lab"
REPO_NAME = "TechTutorPlay-Lab-Core"

# L'URL base per le API delle Issues di GitHub
URL = f"https://api.github.com/repos/{REPO_OWNER}/{REPO_NAME}/issues"

# Intestazioni necessarie per l'autenticazione e per usare le nuove funzioni API
headers = {
    "Authorization": f"token {TOKEN}",
    "Accept": "application/vnd.github+json"
}

# Funzione per creare un nuovo obiettivo (Issue) nel Lab
def crea_obiettivo(titolo, descrizione):
    payload = {
        "title": titolo,      # Il titolo dell'obiettivo (es. "Test RTX 5070")
        "body": descrizione,  # La spiegazione dettagliata dell'attività
        "labels": ["R&D", "Infrastruttura"] # Etichette per organizzare il lavoro
    }
    
    # Invia la richiesta POST a GitHub per creare l'Issue
    response = requests.post(URL, headers=headers, data=json.dumps(payload))
    
    if response.status_code == 201:
        print(f"✅ Obiettivo '{titolo}' creato con successo su GitHub!")
    else:
        print(f"❌ Errore: {response.status_code}")
        print(response.text)

# --- ESECUZIONE ---
# Creiamo il primo obiettivo per testare il sistema
crea_obiettivo(
    "Implementazione Test Benchmark RTX 5070", 
    "Configurare lo script Python per misurare i TFLOPS sulla nuova scheda video del Lab."
)

input("Premi INVIO per chiudere")
