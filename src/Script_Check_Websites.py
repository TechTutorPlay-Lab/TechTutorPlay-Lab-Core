# Per prima cosa, dobbiamo importare 'requests', una libreria che permette a Python di parlare con il web.
# Se non l'hai installata, nel terminale dovresti scrivere: pip install requests
import requests 

# Creiamo una lista (un elenco) dei tuoi domini. 
# In Python le liste si scrivono tra parentesi quadre [].
miei_siti = ["https://www.techtutorplay.tech", "https://techtutorplay.com"]

print("--- Inizio scansione TechTutorPlay Lab ---")

# Usiamo un ciclo 'for' per dire a Python: "Prendi ogni sito nella lista e fai la stessa operazione".
for sito in miei_siti:
    
    # Usiamo 'try' e 'except'. È come dire a Python: "Prova a fare questo, se c'è un errore non crashare, ma fai altro".
    try:
        # 'requests.get' invia un segnale al sito. Il risultato viene salvato nella variabile 'risposta'.
        risposta = requests.get(sito, timeout=5) # timeout=5 dice: "Se dopo 5 secondi non risponde, molla".
        
        # Ogni sito risponde con un codice numerico. 200 significa "Tutto OK".
        if risposta.status_code == 200:
            print(f"✅ {sito} è ONLINE! Codice risposta: {risposta.status_code}")
        else:
            # Se il codice è diverso da 200 (es. 404), c'è un problema.
            print(f"⚠️ {sito} risponde con errore. Codice: {risposta.status_code}")
            
    except Exception as errore:
        # Se ad esempio non hai internet o il dominio è scaduto, 'requests' fallisce e finiamo qui.
        print(f"❌ Impossibile raggiungere {sito}. Errore tecnico: {errore}")

print("--- Scansione completata ---")
input("Premi Invio per uscire...") # Questo serve a tenere aperta la finestra del terminale fino a quando non premi Invio.
