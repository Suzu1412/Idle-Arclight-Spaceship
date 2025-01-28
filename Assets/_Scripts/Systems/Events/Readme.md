# Requisitos

# Script requeridos

## Eventos

- BaseGameEvent.cs

## Listener

- BaseGameEventListener

# Como usar

Tras tener los Scripts creados podrás crear dos Scriptables objects.

El Evento se dispara desde cualquier Script, por ejemplo un cambio en la salud de tu PJ.

El Event Listener (Channel) es el que escucha a dicho evento.
Cada Script que tenga un Channel podrá ejecutar un código al recibir dicho evento. 
De esta forma se desacoplan los Script dado que no necesitan estar en escena para funcionar. 

Forma de usarlo:

En el script que lanzará el evento agrega la variable privada:

```[SerializeField] private SaveGameEvent _saveGameEvent = default;```

Para ejecutarlo usa el comando:

```_saveGameEvent.RaiseEvent(evento);```

En el script que escucha agrega una variable privada Event Listener que escuche al evento.

```[SerializeField] private SaveGameEventListener _saveGameEventListener = default;```

Luego para que asignar el método que escuchará a dicho evento pon en OnEnable:

```_saveGameEventListener.Register(SaveData);```

Y en el OnDisable

```_saveGameEventListener.DeRegister(SaveData);```

Siempre asegúrate de suscribir y desuscribir los eventos para no provocar problemas.


# Como crear nuevos tipos

1. Crea un nuevo archivo, coloca un nombre descriptivo para el archivo que termine con GameEvent o GameEventListener
2. Expande el archivo con la clase que ampliaras como el siguiente ejemplo:

```[CreateAssetMenu(menuName = "Events/Event/Save Event", fileName = "Save Event")]```
public class SaveGameEvent : BaseGameEvent<Save>```
{
}```

