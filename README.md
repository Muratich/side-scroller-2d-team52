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
      - ``` gitGraph
   commit id:"4993881 Initialization"
   commit id:"7c64e5c feat: added player sprite and walk animation"
   branch stg-3
   commit id:"d41054e feat: added enemies system and melee/ranged enemies"
   commit id:"77ece9b (Release3) feat: added level 1 and music"
   checkout main
   merge stg-3 id:"57e5ef4 Merge PR#18 from stg-3"

   commit id:"131fd7d feat: added original sprites for level 1 by Julia"
   commit id:"16a0f92 feat: added lobby and ability to play together"
   commit id:"8525397 feat: tests"
   branch stg-4
   commit id:"1931b07 fix: tests deleted"
   commit id:"988a618 (Release4) fix: unit tests added"
   checkout main
   merge stg-4 id:"37932a8 Merge PR#29 from stg-4"

   branch Muratich-patch-1
   commit id:"29a9506 Create README.md"
   checkout main
   merge Muratich-patch-1 id:"80baf32 Merge PR#30 from Muratich-patch-1"

   branch stg-5
   commit id:"3c60faa feat: enemies synchronization"
   commit id:"3162b45 feat: melee enemy projectile added"
   commit id:"586621a feat: Level 1 for Multiplayer created"
   commit id:"dae72d5 feat: 10 unit tests"
   commit id:"ba22692 fix: suggestion for stg-5"
   checkout main
   merge stg-5 id:"b137fef Merge PR#36 from stg-5"

   commit id:"89cf748 (Release5) Update main.yaml"

   branch slant14-patch-1
   commit id:"2911bfb Rename .yaml to main.yaml"
   checkout main
   merge slant14-patch-1 id:"ecc90df Merge PR#35 from slant14-patch-1"

   commit id:"f52683f feat: menu and singleplayer mode added"
   commit id:"0fa5833 feat: added default weapon for players"

   branch stg-6
   commit id:"ad512de fix: bosses fix and level system fix"
   commit id:"d743824 fix: enemies and boss 1 fix in behaviour and stats"
   commit id:"832ca3c fix: level 2 boss finished"
   commit id:"4f6f369 feat: puzzle templates added"
   commit id:"ff7b583 fix: music fix"
   checkout main
   merge stg-6 id:"ac9e7b9 Merge PR#50 from stg-6"

   commit id:"8618ab5 Added logo"
   commit id:"f4a9a68 Delete logo.png"
   commit id:"db13e70 Add files via upload"
   commit id:"ee63d9b Update README.md"
   commit id:"f0ec3a9 Update README.md"
   commit id:"8fa1238 Update README.md"
   commit id:"f95e5a6 Update README.md"
   commit id:"1b21e47 Create Img"
   commit id:"63dfaa6 Delete Img"
   commit id:"2cb4a15 (origin/master, origin/HEAD) Create template.md" ```
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
