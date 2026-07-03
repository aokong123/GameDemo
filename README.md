## 项目架构


### 单一入口点

本项目遵循 **单一入口点（SEP）** 方法论。

1. 首先加载的场景始终是 **CoreScene**，它几乎为空。起始场景在 **DefaultSceneSelector** 脚本中配置。

2. 对于**绑定（Bindings）**步骤，使用 **Zenject**，它提供了便捷的**依赖注入（Dependency Injection / DI）**系统。
   首先，Zenject 在场景加载后自动连接绑定。这在 **Installers** 脚本中完成。每个场景都有其对应的 Installer。
   例如：**CoreScene** 有其 **CoreInstaller**，**LobbyScene** 有其 **LobbyInstaller**。

3. 接下来是**初始化（Initialization）**步骤。每个场景都有其对应的 **Initiator**，负责初始化 **Enter** 和 **Exit** 入口点。
   例如：**CoreScene** 有其 **CoreInitiator**。
   **注意：CoreInitiator 是游戏中唯一拥有 Start 方法的脚本**

---

## 职责划分
项目中的脚本可以分为多个类别，每个类别都有其明确的职责。

### 1. Installer
- **始终**尽可能绑定到接口（Interfaces），但为什么呢？
  a. 某些类必须是接口，因为需要在游戏过程中将它们绑定到不同的实例。因此，为了保持项目整体的一致性，即使在并非严格必要的地方也会使用接口。
  b. 接口清晰地揭示了类的公开方法（public methods）和用途。
  c. 可以在单元测试（Unit Tests）中注入 Mock 实例。
  d. 遵循**依赖反转模式（Dependency Inversion Pattern）**依赖抽象（abstractions），因为永远不知道何时会需要它们，回顾我自己的游戏开发经历，深刻体会到了软件开发中唯一不变的就是"一切都在变"。

### 2. Initiator
- 提供场景的单一 Entry 和 Exit 入口点。

### 3. Commands
- 游戏功能的"指挥者"。**Command** 是唯一被允许引用其他功能（**Controllers**）的类。
- 本项目不触发任何事件（events），取而代之的是**执行 Commands**。
- Command 是一个带有 **Execute** 方法的简单类，通过多个脚本之间的通信，按特定顺序调用所需的逻辑。
- **要注意的是：** 当一个类执行 Command，而该 Command 的某个效果又更新了该类时，可能会出现**循环依赖（Circular Dependencies）**。为了避免 Commands 中的循环依赖，每个 Command 自行从 **DIContainer** 解析其引用。

### 4. MVC
在应用程序和游戏中使用的软件架构模式，将功能类分为 3 组：

- **Model（模型）：** 一个只包含数据的简单类，例如字段（fields）、属性（properties）和小型的条件方法（conditional methods）。我选择不在 Model 中包含任何 Event/Action，为此使用 Commands

- **View（视图）：** 表示对象的视觉元素/组件的类，例如 Sprite / ParticleSystem / 3D Model / UI。它**始终**继承自 **MonoBehaviour** 并挂载在 GameObject 上。
  View 只能引用更小的内部 View。它从不自行调用任何东西，只接收指令或将回调传递给 Controller。

- **Controller（控制器）：** 游戏中功能的"大脑"。它查询 Model，相应地更新 View，并为复杂流程调用 Commands。 Controller 是唯一被允许引用其 Model 和 View 的类。
  例如：**ArrowController** 控制 **ArrowView**。

---

## 5. Services
- Service 没有 View，也不影响其他脚本。其唯一职责是向其他脚本提供通用功能。
- Service 可以从 Controllers / Commands / Services 中访问。
- 如果数据需要从多处访问，不应将其存储在独立的 DataService 中（这相当于一个公共 Model）。例如：**LevelDataService**。

### 6. Factory
- 封装创建特定对象的逻辑。例如：**LevelFactory**。

### 7. Utils
- 包含通用、可复用功能的静态类。

### 8. Extensions
- 提供扩展方法（extension methods）的静态类。

### 9. Game State
- 表示游戏的一个阶段，具有 Enter 和 Exit 方法，用于**状态机（StateMachine）**模式。游戏在任何时刻只能处于一个 GameState。
- 有 2 个 GameState：**Lobby State** 和 **GamePlay State**。

---

## 项目结构

本项目遵循 **Domains 项目结构** 来组织文件夹，也就是按域划分。

### 1. 项目中的 Domains：

- **Core：** 包含所有非本项目专用的脚本/资源，可复用于其他游戏。如上所述，CoreScene 是游戏的第一个场景。
- **Game：** 包含所有专用于此特定游戏的脚本/资源，可在多个场景/状态中使用。GameScene 加载在 CoreScene 之上。
- **Lobby / GamePlay：** 代表 GameState 的 Domain。加载在 CoreScene 之上。

### 2. Assembly Definitions
- 每个 Domain 都有其自己的 **Assembly Definition**，强制实现 Domain 之间的引用隔离。
  例如：Core Domain 有 **CoreAssembly**。
- 这样做可以减少脚本编译时间，因为编译器只需要重新编译发生变化的 Assemblies。

---

## 设计模式与实践

### 技术要点

#### 1. UpdateSubscriptionService（UpdateManager）
- 本项目使用 **UpdateManager** 模式来提升**性能**，并允许任何脚本订阅 **Update / FixedUpdate / LateUpdate** 方法。

#### 2. 摄像机分离（Cameras Separation）
- 使用**2 台摄像机**：一台用于渲染世界，另一台用于渲染 UI，以确保 **Post-Processing** 或其他调整不会影响 UI。

#### 3. 对象池（Object Pooling）
- 在游戏过程中频繁创建的对象被回收利用，以减少内存分配。
- 例如：**ScoreGainedFXPool** 回收 **ScoreGainedFXView**。

#### 4. 资源加载（Asset Loading）
- 本项目使用 Unity 的 **Addressable System** 来动态加载资源。
- **注意：** 所有资源目前都是项目的一部分，但可以轻松地移至外部服务器并**远程加载**。
- **本项目解决方案：** 多次构建 Bundles，每个版本构建一次。每个线上版本将加载其对应的 bundles，不会出现问题。
  **替代方案：** 只构建原始资源（如图片、3D 模型、音频文件或不附加脚本的 prefab）。

#### 5. 单元测试（Unit Tests）
- 本项目仅对其核心逻辑 Service 进行单元测试覆盖。这可以扩展到更多类。
- 作为 **CI/CD** 流水线的一部分，**PreBuildUnitTestsValidator** 类在任何单元测试失败时会使构建失败。

#### 6. 动画（Animations）
- 本项目使用 **Legacy Animation** 来执行简单动画，其性能显著更优——尤其对于 UI 对象。
  **注意：** 如果有复杂的动画对象（如人形角色），则会改用 **Animator** 组件。

#### 7. Prefabs
- 大多数对象都是 prefab，以避免多个开发者并行工作时发生场景冲突。

#### 8. 光照烘焙（Lights Baking）
- 环境光照是预先烘焙的，以提升运行时性能。

#### 9. Draw Calls 合批（Draw Calls Batching）
- 环境中的所有对象使用单一材质，使其能够在一次 draw call 中渲染，提升性能。

#### 10. Shaders
- 在可能的情况下，使用 shader 代替动画。例如，中水平滚动的山脉背景，因为 shader 计算在 GPU 上并行运行，比基于 CPU 的动画快得多。

#### 11. 日志（Logs）
- 整个代码库都有日志监控，以便于调试。

#### 12. Tween 动画
- 使用 **DoTween** 包通过代码创建简单的 tween 动画。
  例如：在 **ArrowMovementController** 中旋转箭头（Arrow）。

#### 13. 持久数据加密（Persistent Data Encryption）
- 为了跨游戏保存数据，使用 Unity 的 **PlayerPrefs**。
- 为了加大破解的难度，对数据进行加密（参见 **PlayerPrefsDataPersistence**）。
- **注意：** 存储玩家数据的理想位置是远程安全服务器。然而，这需要满足几个重要的前提条件：
  i. 需要管理自己的远程数据库，或付费使用云存储服务。
  ii. 要获取每个玩家的数据，需要玩家的唯一标识符，这意味着必须实现登录/身份认证系统。
  iii. 这需要一个小型后端服务器来处理登录验证。
  iv. 因此，还需要托管该服务器——无论是本地托管还是付费使用远程机器。
  这是商业游戏避免依赖 PlayerPrefs 的标准做法。像这样的小型demo项目中，不太现实。

---

## 项目规范

### 编码标准

1. 本项目主要遵循 **Microsoft 编码规范（Microsoft's Coding Conventions）**。

### 异步操作（Async Operations）

1. 本项目使用 **Awaitables**——Unity 中异步操作的全新改进原生解决方案。
2. 由于异步操作默认不捕获异常，每次启动异步操作时，**必须**使用 **try/catch** 块将其包裹。
3. 为项目中的每个异步操作传递 **Cancellation Tokens**，使能够在需要时取消操作。
4. 游戏中的每个关卡都有其自己的 **CancellationToken**，保存在 **LevelCancellationTokenService** 中，当关卡被销毁（disposed）时该 token 会被取消。

### Clean Code 原则

本项目采纳 **Clean Code** 原则

1. **编写自文档化代码（Self-Documenting Code）**
   - 代码应当易于阅读，无需注释即可讲述其故事——使用有意义的命名和可复用的函数。
     注释仅用于没有明确替代方案的警告场景。

2. **单一职责原则（Single Responsibility Principle / SRP）**
   - 每个类、方法或模块应当有且仅有一个变化的原因。

3. **小而专注的函数和类**

4. **避免深层 if 嵌套**
   - 尽可能保持分支逻辑扁平化。

5. **避免 switch 语句**
   - 用适当的抽象（abstractions）替代它们。

6. **DRY（Don't Repeat Yourself——不要重复自己）**
   - 避免代码重复，但仅在确实不必要的时候。

7. **不可变的 Get 方法**
   - Get 方法永远不应修改内部状态或产生副作用。

8. **不使用"魔法数字"**
   - 所有常量变量都存储为 const。
