#CHAOS CLIMB
![[logo.png]]

## Usage
The MVP 2 project is a multiplayer 2d side-scroller, in which we added multiplayer, synchronized opponents between the client and the host, and also added an action game with two types of weapons.

To access the product, you need to download it from the https://marat-diyarov.itch.io/summer-project-2025-team-52 and unpack it. Then launch it .exe file.

## Architecture
The architecture of the project is modular in those places where it is needed. For example, inexperienced developers often write all the logic in one script when creating opponents. In our game, each opponent is "assembled" from separate components, for example, a melee opponent consists of components: movement by points, melee attack, inspection.

### Static view
Mostly architecture of project dscribed in the Miro board: https://miro.com/app/board/uXjVIrXDYK0=/

### Deployment view
The game runs in a peer‑to‑peer LAN model: each player launches the same executable and exchanges input data over a single UDP port (e.g. 7777) to synchronize physics, animations and events. For seamless host discovery, clients send simple UDP broadcasts on the local switch/router, with manual IP entry as a fallback; only one port needs to be opened in the firewall. This setup requires no external servers, minimizes latency and is trivially deployed on the customer’s network.

## Development
### Kanban board
- [Link to kanban board](https://miro.com/app/board/uXjVIrXDYK0=/)

### Git workflow

- Specify which base workflow you adapted to your context (e.g., GitHub flow, Gitflow, etc).

- Explain / Define rules for:
   - each contributor can create and close issues
   - issues must include acceptance criteria, priority and story points (as labels)
   - each branch should be "stg-{current sprint number}" (e.g "stg-5")
   - to merge "stg-{sprint number}" to "main" we need acceptance from customer
   - commits should satisfy [commit style convention](https://gist.github.com/qoomon/5dfcdf8eec66a051ecd85625518cfd13)
   - pull request must be created by Marat Diiarov and accepted by customer
  


### Automated tests
The tests were conducted using the Unity Test Framework (UTF).
10 tests were written affecting different game mechanics (from the camera to the player's spawner).
All tests are written in C# and are located in the Assets/Tests folder.

## Build and deployment
Every MVP deployed to itch.io according to the curtomer requirement.
