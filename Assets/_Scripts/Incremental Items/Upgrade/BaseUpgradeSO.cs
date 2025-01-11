using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("Suzu.Manager")]
public abstract class BaseUpgradeSO : SerializableScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    [SerializeField] protected double _cost;
    [SerializeField] protected FloatVariableSO _upgradeCostMultiplier = default;

    [SerializeField] protected FloatModifier _modifierToApply;
    [SerializeField] protected VoidGameEvent OnProductionChangedEvent;
    protected DoubleVariableSO _finalCost;
    [SerializeField] protected bool _isApplied;
    [SerializeField] protected bool _isRequirementMet;

    [Header("Notification")]
    [SerializeField] private NotificationGameEvent OnShopNotificationEvent;
    [SerializeField] private Sprite _notificationIcon;

    protected FormattedNumber CostFormatted { get; private set; }
    private NotificationSO _notification;

    protected bool _isDirty;

    public string Name => _name;
    public string Description => _description;
    public float AmountToModify => _modifierToApply.Value;
    public bool IsRequirementMet { get => _isRequirementMet; internal set => _isRequirementMet = value; }
    public bool IsApplied => _isApplied;

    public DoubleVariableSO Cost
    {
        get
        {
            _finalCost.Value = _cost * _upgradeCostMultiplier.Value;
            return _finalCost;
        }
    }

    protected virtual void OnEnable()
    {
        _finalCost = ScriptableObject.CreateInstance<DoubleVariableSO>();
        _finalCost.Initialize(0, 0, double.MaxValue);
        _modifierToApply.SetSource(name);

        _notification = ScriptableObject.CreateInstance<NotificationSO>();

    }


    private void OnDisable()
    {
        RemoveUpgrade();
        _isApplied = false;
    }

    internal void Initialize()
    {
        IsRequirementMet = false;
        ApplyUpgrade(false);
    }

    internal virtual void BuyUpgrade(double currency)
    {
        if (currency >= Cost.Value)
        {
            ApplyUpgrade(true);
        }
    }

    public void UnlockRequirements(double currency)
    {
        if (CheckIfMeetRequirementToUnlock(currency))
        {
            IsRequirementMet = true;
            Notificate();
        }
    }

    internal abstract void ApplyUpgrade(bool val);

    internal abstract void RemoveUpgrade();

    public FormattedNumber GetCost()
    {
        CostFormatted = FormatNumber.FormatDouble(Cost.Value, CostFormatted);
        return CostFormatted;
    }

    protected abstract bool CheckIfMeetRequirementToUnlock(double currency);

    private void Notificate()
    {
        _notification.SetMessage("newUpgradeNotification");
        _notification.SetSprite(_notificationIcon);
        OnShopNotificationEvent.RaiseEvent(_notification);

    }
}
