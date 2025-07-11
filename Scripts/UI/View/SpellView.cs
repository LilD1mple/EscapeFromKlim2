using DG.Tweening;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Inputs.Interfaces;
using EFK2.WeaponSystem.Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.UI.View
{
    public class SpellView : MonoBehaviour, IEventReceiver<InputKeyChangedSignal>
    {
        [Header("Source")]
        [SerializeField] private SpellPresenter _spallAttack;

        [Header("Icon Variants")]
        [SerializeField] private Sprite _chargedSpellIcon;
        [SerializeField] private Sprite _unchargedSpellIcon;

        [Header("Components")]
        [SerializeField] private Image _magicIcon;
        [SerializeField] private Image _spellChargeFill;
        [SerializeField] private TMP_Text _magicSpellKeyText;

        [Header("Constants")]
        [SerializeField] private float _fillAlphaChannel;
        [SerializeField] private string _spellKeyName;

        private EventBus _eventBus;

        private IKeyboardUpdateService _keyboardUpdateService;

        UniqueId IBaseEventReceiver.Id => new();

        private void Start()
        {
            UpdateKey(_keyboardUpdateService.KeyValues[_spellKeyName]);
        }

        private void OnEnable()
        {
            _eventBus.Subscribe(this);

            _spallAttack.RemainingTimeChanged += OnRemainingTimeChanged;

            _spallAttack.ChargeChanged += OnSpellChargeChanged;
        }

        private void OnDisable()
        {
            _eventBus.Unsubscribe(this);

            _spallAttack.RemainingTimeChanged -= OnRemainingTimeChanged;

            _spallAttack.ChargeChanged -= OnSpellChargeChanged;
        }

        [Inject]
        public void Construct(IKeyboardInputService keyboardInputService, EventBus eventBus)
        {
            _keyboardUpdateService = keyboardInputService.UpdateService;

            _eventBus = eventBus;
        }

        private void OnRemainingTimeChanged(float value)
        {
            _spellChargeFill.fillAmount = value;
        }

        private void OnSpellChargeChanged(bool charge)
        {
            _magicIcon.sprite = charge ? _chargedSpellIcon : _unchargedSpellIcon;

            _spellChargeFill.DOFade(charge ? 0f : _fillAlphaChannel, 0f);

            _magicSpellKeyText.enabled = charge;
        }

        private void UpdateKey(KeyCode keyCode)
        {
            _magicSpellKeyText.text = keyCode.ToString();
        }

        void IEventReceiver<InputKeyChangedSignal>.OnEvent(InputKeyChangedSignal @event)
        {
            if (@event.Key == _spellKeyName)
                UpdateKey(@event.KeyCode);
        }
    }
}