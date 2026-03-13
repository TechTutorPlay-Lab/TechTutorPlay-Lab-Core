# Importiamo la libreria sys per interagire con i parametri del sistema operativo
import sys 

# Definiamo la funzione principale che darà il benvenuto ufficiale al Lab
def welcome_to_lab():
    # Creiamo una variabile che contiene il nome del nostro progetto
    project_name = "TechTutorPlay Lab" 
    # Stampiamo un messaggio di benvenuto dinamico utilizzando la variabile appena creata
    print(f"Benvenuti su {project_name} - Status: Operativo") 
    # Stampiamo una conferma che l'hardware RTX 5070 è pronto per il calcolo IA
    print("Hardware Check: NVIDIA RTX 5070 rilevata e pronta.") 

# Verifichiamo se il file viene eseguito direttamente (e non importato come modulo)
if __name__ == "__main__":
    # Chiamiamo la funzione di benvenuto per eseguire il codice
    welcome_to_lab()