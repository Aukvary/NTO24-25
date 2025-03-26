using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NTO24.UI
{
    public class ChangeBearMenu : Drawable
    {
        [SerializeField]
        private int _bearNum;

        private IconHUD[] _icons;
        private Button[] _buttons;

        public static List<ChangeBearMenu> Instance = new(); 

        protected override void Awake()
        {
            Instance.Add(this);
        }

        public static void Initialize()
        {
            foreach (var item in Instance) 
                item.Init();
        }

        public void Init()
        {
            _icons = GetComponentsInChildren<IconHUD>();
            _buttons = _icons.Select(i => i.GetComponent<Button>()).ToArray();

            _buttons[0].onClick.AddListener(() =>
            {
                BearInfo.Bears[_bearNum] = null;

            });

            foreach (var button in _buttons)
            {
                button.onClick.AddListener(() =>
                {
                    foreach (var but in _buttons)
                        foreach (var renderer in but.GetComponentsInChildren<Image>())
                        {
                            var color = renderer.color;
                            color.a = 0.5f;
                            renderer.color = color;
                        }

                    foreach (var image in button.GetComponentsInChildren<Image>())
                    {
                        var color = image.color;
                        color.a = 1;
                        image.color = color;
                    }
                });
            }

            for (int i = 1; i < _buttons.Length; i++)
            {
                int index = i;
                _buttons[i].onClick.AddListener(() =>
                {
                    BearInfo.Bears[_bearNum] = BearInfo.bearInfos[index - 1];
                });
            }

            for (int i = 1; i < _icons.Length; i++)
            {
                if (i <= BearInfo.bearInfos.Count)
                    _icons[i].Icon = new Sprite[] { Sprite.Create(BearInfo.bearInfos[i - 1].Icon,
                    new Rect(new(0f, 0f),
                    new (BearInfo.bearInfos[i - 1].Icon.width, BearInfo.bearInfos[i - 1].Icon.height)),
                    new(0.5f, 0.5f)) };
                else
                    _icons[i].gameObject.SetActive(false);
            }

            foreach (var button in _buttons)
            {
                foreach (var renderer in button.GetComponentsInChildren<Image>())
                {
                    var color = renderer.color;
                    color.a = 0.5f;
                    renderer.color = color;
                }
            }

            foreach (var renderer in _buttons[0].GetComponentsInChildren<Image>())
            {
                var color = renderer.color;
                color.a = 1f;
                renderer.color = color;
            }
        }
    }
}