# **QASs**
## Reliability
### Recoverability
**Importance to customer:** Players need to resume gameplay quickly after failures without losing progress. In a co-op game with complex boss fights, recoverability ensures frustration-free sessions and maintains immersion.

#### Scenario: Network Crash Recovery
- **Source:** Network infrastructure  
- **Stimulus:** Sudden LAN disconnection during boss fight  
- **Artifact:** Game session state system  
- **Environment:** Co-op mode, Level 2 boss battle  
- **Response:**  
  1. Auto-save player positions/health  
  2. Preserve boss health state  
  3. Display reconnection prompt  
- **Response Measure:**  
  - 100% session state recovery  
  - ≤10s reconnection time  
  - 0% progress loss  

**Execution:**  
1. Simulate network dropout using Unity Test Framework  
2. Verify save files in `%APPDATA%/GameName/saves`  
3. Measure time to restore session  

#### Scenario: Progress Corruption Recovery
- **Source:** System crash  
- **Stimulus:** Game freeze during auto-save  
- **Artifact:** Save file system  
- **Environment:** After defeating level 1 boss  
- **Response:**  
  1. Maintain backup saves  
  2. Auto-repair corrupted files  
  3. Restore most recent valid state  
- **Response Measure:**  
  - 99% save integrity rate  
  - ≤1 backup loss maximum  

**Execution:**  
1. Inject file corruption via debug console  
2. Verify backup loading from `UserData/Saves/backups`  
3. Validate checksums via MD5 verification  

## Flexibility
### Installability
**Importance to customer:** Players on different OSes should install with one click from itch.io. Simple installation increases accessibility and reduces support requests.

#### Scenario: Cross-Platform Installation
- **Source:** itch.io download page  
- **Stimulus:** Player downloads game package  
- **Artifact:** Game installer  
- **Environment:** Windows/Mac/Linux systems  
- **Response:**  
  1. Single-file executable launch  
  2. Auto-detected dependencies  
  3. Desktop shortcut creation  
- **Response Measure:**  
  - 100% launch success rate  
  - ≤60s installation time  
  - 0 manual dependency installs  

**Execution:**  
1. Test on clean VMs (Win10, macOS Monterey, Ubuntu 22.04)  
2. Time installation process  
3. Verify launch via `GameName.exe --test-install`  

#### Scenario: Storage Optimization
- **Source:** Low-disk-space systems  
- **Stimulus:** Installation on ≤2GB free space  
- **Artifact:** Asset loading system  
- **Environment:** HDD storage devices  
- **Response:**  
  1. Dynamic texture downscaling  
  2. Optional asset installation  
  3. Clear storage requirements  
- **Response Measure:**  
  - ≤1.5GB total footprint  
  - Adaptive texture quality (SD/HD)  

**Execution:**  
1. Simulate disk constraints via Unity's DiskFullSimulator
2. Monitor memory usage with Unity Profiler  

## Compatibility
### Interoperability
**Importance to customer:** Seamless LAN co-op between different hardware ensures friends can play together regardless of their PC setups.

#### Scenario: Cross-Device LAN Play
- **Source:** Player 2's gaming laptop  
- **Stimulus:** Join request from different hardware  
- **Artifact:** Network synchronization system  
- **Environment:**  
  - Player 1: Windows/RTX 3060  
  - Player 2: macOS/M1 Chip  
- **Response:**  
  1. Auto-resolution scaling  
  2. Input scheme adaptation  
  3. Latency compensation  
- **Response Measure:**  
  - ≤50ms input latency  
  - 99% frame sync accuracy  
  - 0% game version mismatch  

**Execution:**  
1. Connect mixed-device test rigs  
2. Run Unity Network Profiler  
3. Verify with `NetworkDiagnostics.OutgoingMessageCount`  

#### Scenario: Peripheral Compatibility
- **Source:** Gamepad/Keyboard hybrid setup  
- **Stimulus:** Simultaneous input from different controllers  
- **Artifact:** Input handling system  
- **Environment:** Co-op session with:  
  - Player 1: Xbox controller  
  - Player 2: Keyboard + mouse  
- **Response:**  
  1. Input device auto-detection  
  2. Control rebinding without restart  
  3. UI adaptation per device  
- **Response Measure:**  
  - 100% input recognition  
  - ≤0.5s control remapping  
  - 0 input conflicts  

**Execution:**  
1. Test with [Unity Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.3/manual/Testing.html)  
2. Simulate device changes via `InputTestFixture`  
3. Verify control mappings in `Input/control_schemes.json`
