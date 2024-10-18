# Requisitos

- Singleton Pattern (Encontrado en Common/Utilities)

# Script requeridos

- ObjectPooler.cs
- ObjectPoolFactory.cs
- ObjectPoolSettingsSO.cs
- Singleton.cs
- ObjectPoolType.cs

# Como usar

Tras agregar a tu proyecto todos los Script necesarios haz lo siguiente:

1. Crea un Scriptable Object de ObjectPoolSettingsSO
2. En el SO creado: Ingresa el tipo de objeto que será y asigna un Prefab correspondiente en sus campos
3. Agregar el enum correspondiente a ObjectPoolType.cs  

Para los Prefabs que serán parte del Pool

1. Para el Script del Prefab debes asegurarte de agregar un componente del tipo ObjectPooler.cs
2. Para devolver el objeto al Pool se debe agregar en el mismo Script: ObjectPoolFactory.ReturnToPool(Pool); 

Para el objeto que instanciará los GameObjects se debe hacer lo siguiente

1. En el Script que instancia agrega: ObjectPoolSettingsSO _pool;
2. Para instanciar el objeto agrega la línea: ObjectPoolFactory.Spawn(_itemPickUpPool); 