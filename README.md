## NETCrazyZombie multijugador

### Mejoras sugeridas:

- Crear el sistema de proyectiles de igual forma que se hace en NETTanks.

  - Prefab base
  - Prefab cliente
  - Prefab servidor

    -- Creado sistema de disparo similar a Nettanks, spawneando balas desde el arma con cun spawnpoint y un prefab cliente + prefab servidor --

- Separar el sistema de salud del jugador de forma similar a NETTanks.

  -- Desacopla sistema de saludo del PlayerManager --

- Separar el sistema del display de salud de forma similar a NETTanks.

- Implementar un sistema de respawn que funcione correctamente.

### Mejoras sugeridas (investigación, avanzado):

- Cambiar el sistema de cámaras, utilizando el paquete Cinemachine. Se utiliza en NETTanks en la rama online.

### Bugs detectados:

- Los zombies se mueven erráticamente en ocasiones cuando chocan con las escaleras y no pueden alcanzar al jugador.
  -- Ajusta rigidbody enemigos para evitar comportamientos de físicas inesperados --
