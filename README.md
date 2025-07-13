# CHAOS CLIMB
![](https://github.com/Muratich/side-scroller-2d-team52/blob/master/logo.png)

☀️ Our project is a multiplayer 2d side-scroller with co-op, puzzles for teamwork and combat with enemies and bosses.

📍 ![Install game](https://marat-diyarov.itch.io/summer-project-2025-team-52)

▶️ ![Demo video](https://drive.google.com/file/d/12WhGG4Ikv21sZW23Yg7ozpP4KMjqLsd_/view?usp=sharing)

## Product
💥 The goal of our project is to create a new 2D multiplayer side-scrolling game for customers, with all the necessary features.

**Project context diagram:**

![](https://github.com/Muratich/side-scroller-2d-team52/blob/master/docs/project/productBoard.png)

**Project roadmap**

![](https://github.com/Muratich/side-scroller-2d-team52/blob/master/docs/project/roadmap.png)

📍 Guide for players:
   - To start singleplayer just press _Singleplayer_ button;
   - To play with friends using LAN, you need to be connected to a shared WiFi network. One player should create a lobby by clicking the _Host_ button, and the other player must enter the IP address of the host and click _Connect_.

📍 Installation Guide:
   - To access the product, you need to download it from the https://marat-diyarov.itch.io/summer-project-2025-team-52 and unpack it. Then launch .exe file.

## Documentation: 📗

### Development 🏭
   - [Link to kanban board](https://miro.com/app/board/uXjVIrXDYK0=/)
   - Workflow:
      - As a base workflow we use GitHub Flow, since we have one "main" and for each sprint we create separate branch and pull requests further.
      - @startuml
!theme plain

' Main classes and relationships
class CameraFollow {
  + float followSpeed
  + float yOffset
  + float zOffset
  + bool isMoving
  + void LateUpdate()
  + void SetTarget(Transform)
}

class SpawnEffect {
  + Transform origin
  + GameObject effect
  + float lifetime
  + void Spawn()
}

class WhiteShade {
  + SpriteRenderer spriteRenderer
  + Color shadeColor
  + void Shade()
  + IEnumerator ShadeCor()
}

class Enemy <<NetworkBehaviour>> {
  + Health health
  + void EnemyDestroyLocal()
  + void RequestDespawnServerRpc()
}

abstract class EnemyAttack <<NetworkBehaviour>> {
  + Animator animator
  + float attackReload
  + Transform attackPos
  + void StartAttackServerRpc(Vector3)
  + void StopAttackServerRpc()
  + void UpdateTargetPositionServerRpc(Vector3)
  + {abstract} void StartAttack(Vector3)
  + {abstract} void StopAttack()
}

class EnemyAttackMelee {
  + GameObject hitZone
  + IEnumerator HitLoop()
}

class EnemyAttackRange {
  + GameObject bulletPrefab
  + float bulletSpeed
  + IEnumerator Fire()
}

class EnemyPatternMovement <<NetworkBehaviour>> {
  + List<Transform> destinationPoints
  + float speed
  + float timeToStayAtPoint
  + bool isDasher
  + void Init(List<Transform>)
  + IEnumerator Movement()
  + IEnumerator DashTo(Vector3)
}

class ViewZone <<NetworkBehaviour>> {
  + float viewRadius
  + float viewAngle
  + Transform enemyObj
  + EnemyAttack enemyAttack
  + void CheckVisibleTargets()
  + Transform GetClosestVisibleTarget()
}

class Health <<NetworkBehaviour>> {
  + int startHealth
  + NetworkVariable<int> CurrentHealth
  + void TakeDamageServerRpc(int)
  + void HealServerRpc(int)
  + IEnumerator Invincibility()
}

class Weapon <<NetworkBehaviour>> {
  + float reloadTime
  + Transform firePos
  + GameObject proj
  + Transform target
  + Transform scaleRef
  + {abstract} void Attack()
}

class SniperWeapon {
  + float bulletSpeed
  + IEnumerator Reload()
  + IEnumerator DeleteBullet(NetworkObject)
}

class SwordWeapon {
  + IEnumerator Reload()
  + IEnumerator DeleteBullet(NetworkObject)
}

class Movement <<NetworkBehaviour>> {
  + bool control
  + float speed
  + Rigidbody2D rb
  + InputHandler input
  + void Move()
  + void Jump()
  + void GroundCheck()
}

class WeaponManager <<NetworkBehaviour>> {
  + Transform holdPoint
  + Weapon currentWeapon
  + void TryPickup(PickUpWeapon)
  + void ExecuteAttack()
}

class LobbyManager {
  + GameObject menuPanel
  + GameObject lobbyPanel
  + void OnHostButtonClicked()
  + void OnClientButtonClicked()
  + void Singleplayer()
}

' Inheritance relationships
EnemyAttack <|-- EnemyAttackMelee
EnemyAttack <|-- EnemyAttackRange
Weapon <|-- SniperWeapon
Weapon <|-- SwordWeapon

' Composition relationships
CameraFollow "1" *-- "1" Transform
Enemy "1" *-- "1" Health
EnemyPatternMovement "1" *-- "1" ViewZone
ViewZone "1" *-- "1" EnemyAttack
Health "1" *-- "n" UnityEvent
Movement "1" *-- "1" InputHandler
Movement "1" *-- "1" WeaponManager
WeaponManager "1" *-- "1" Weapon
LobbyManager "1" *-- "1" UnityTransport

' Association relationships
EnemyPatternMovement "1" --> "n" Transform : destinationPoints
ViewZone "1" --> "n" Transform : tracks targets
WeaponManager "1" --> "1" PickUpWeapon
SpawnEffect "1" --> "1" GameObject : spawns
WhiteShade "1" --> "1" SpriteRenderer : affects

' Network-related notes
note top of Enemy
  Uses NetworkBehaviour
  for multiplayer sync
end note

note top of Health
  NetworkVariable for
  health synchronization
end note

note bottom of WeaponManager
  Spawns weapons over network
  with ownership management
end note

@enduml
      - Git workflow rules
         - each contributor can create and close issues
         - issues must include acceptance criteria, priority and story points (as labels)
         - each branch should be "stg-{current sprint number}" (e.g "stg-5")
         - to merge "stg-{sprint number}" to "main" we need acceptance from customer
         - commits should satisfy [commit style convention](https://gist.github.com/qoomon/5dfcdf8eec66a051ecd85625518cfd13)
         - pull request must be created by Marat Diiarov and accepted by customer
   - Secret management
      - In the secrets of repository on GitHub, we store the Unity license.

   
### Quality assurance
#### Quality attribute scenarios
![Link to the quality attribute scenarios documentation](https://github.com/Muratich/side-scroller-2d-team52/blob/master/docs/quality-assurance/quality-attribute-scenarios.md)

#### Automated tests
The tests were conducted using the Unity Test Framework (UTF). 10 tests were written affecting different game mechanics (from the camera to the player's spawner). All tests are written in C# and are located in the Assets/Tests folder.
#### Tools Used
Unit framework for Unity UTF
#### Types of Tests Implemented and their locations
Unit tests: [Assets/Tests/](Assets/Tests/)
#### User acceptance tests
![link to  user acceptance tests file](https://github.com/Muratich/side-scroller-2d-team52/blob/master/docs/quality-assurance/user-acceptance-tests.md)

### Build and deployment automation
#### Continuous Integration
   -  [main.yaml]
   - [Workflow file](https://github.com/Muratich/side-scroller-2d-team52/blob/master/.github/workflows/main.yaml)
   - Testing tools:
     - Unity Test Runner: "Execute unit and integration tests"

### Architecture 🏢

The architecture of the project is modular in those places where it is needed. For example, inexperienced developers often write all the logic in one script when creating opponents. In our game, each opponent is "assembled" from separate components, for example, a melee opponent consists of components: movement by points, melee attack, inspection.

**Static view**
   - Mostly architecture of project dscribed in this [diagram](https://miro.com/app/board/uXjVIrXDYK0=/?moveToWidget=3458764634450580967&cot=14) of Miro board.

**Dynamic view**
   - Dynamic view temp

**Deployment view**
   - The game runs in a peer‑to‑peer LAN model: each player launches the same executable and exchanges input data over a single UDP port (e.g. 7777) to synchronize physics, animations and events. For seamless host discovery, clients send simple UDP broadcasts on the local switch/router, with manual IP entry as a fallback; only one port needs to be opened in the firewall. This setup requires no external servers, minimizes latency and is trivially deployed on the customer’s network.

**Tech stack**
   - Unity 6 (version 6000.0.39f1)
   - Unity Netcode for GameObjects


### Automated tests

The tests were conducted using the Unity Test Framework (UTF).
10 tests were written affecting different game mechanics (from the camera to the player's spawner).
All tests are written in C# and are located in the Assets/Tests folder.

### Build and deployment

Every MVP deployed to itch.io according to the curtomer requirement.
