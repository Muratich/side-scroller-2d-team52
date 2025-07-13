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
   - ![](https://github.com/Muratich/side-scroller-2d-team52/blob/master/.github/workflows/gitgrapg.png)
   - Git workflow rules
      - each contributor can create and close issues
      - issues must include acceptance criteria, priority and story points (as labels)
      - each branch should be "stg-{current sprint number}" (e.g "stg-5")
      - to merge "stg-{sprint number}" to "main" we need acceptance from customer
      - commits should satisfy [commit style convention](https://gist.github.com/qoomon/5dfcdf8eec66a051ecd85625518cfd13)
      - pull request must be created by Marat Diiarov and accepted by customer
   - Secret management
      -   
### Quality characteristics and quality attribute scenarios
### Quality Assurance
### Build and deployment automation

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
