using UniMediator.Runtime;
using UniMediator.Runtime.VContainer;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private InteractionConfig _interactionConfig;

    [SerializeField] private HUDController _hudController;
    [SerializeField] private DialogueUIController _dialogueUI;
    [SerializeField] private PlayerPivotsProvider _playerPivotsProvider;

    private PlayerInputActions _inputActions;

    protected override void Configure(IContainerBuilder builder)
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();

        builder.RegisterInstance(_inputActions).AsSelf();
        builder.RegisterInstance(_playerConfig);
        builder.RegisterInstance(_interactionConfig);


        builder.RegisterMediator();

        builder.Register<PlayerState>(Lifetime.Singleton);
        builder.Register<InteractionService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<ItemHolder>(Lifetime.Singleton);
        builder.Register<QuestManager>(Lifetime.Singleton);

        builder.RegisterComponent(_hudController);
        builder.RegisterComponent(_dialogueUI);

        builder.RegisterComponent(_playerPivotsProvider).As<IItemHolderPivots>();

        builder.RegisterComponentInHierarchy<PlayerController>();
        builder.RegisterComponentInHierarchy<FirstPersonCamera>();

        //Better spawn with some npc manager
        builder.RegisterComponentInHierarchy<NPCController>();

        builder.RegisterComponentInHierarchy<ValveController>();
        builder.RegisterComponentInHierarchy<ChestController>();
    }

    protected override void Awake()
    {
        base.Awake();

        foreach (var socket in FindObjectsByType<ItemSocket>(FindObjectsSortMode.None))
        {
            Container.Inject(socket);
        }
    }

    protected override void OnDestroy()
    {
        _inputActions.Player.Disable();
        _inputActions.UI.Disable();
        _inputActions.Disable();
        _inputActions.Dispose();

        base.OnDestroy();
    }
}