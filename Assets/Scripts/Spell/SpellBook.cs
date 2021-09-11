using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Spell
{
    public class SpellBook : MonoBehaviour
    {
        [SerializeField] private Image castingBar;
        [SerializeField] private TMP_Text currentSpell;
        [SerializeField] private TMP_Text castTime;
        [SerializeField] private Image icon;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Spell[] spells;

        private static SpellBook instance;

        private Coroutine spellRoutine;
        private Coroutine fadeRoutine;

        public static SpellBook Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<SpellBook>();

                return instance;
            }
        }

        public Spell CastSpell(string spellName)
        {
            Spell spell = Array.Find(spells, x => x.Name == spellName);

            castingBar.fillAmount = 0.0f;
            castingBar.color = spell.BarColor;
            currentSpell.text = spell.Name;
            icon.sprite = spell.Icon;

            spellRoutine = StartCoroutine(Progress(spell));
            fadeRoutine = StartCoroutine(FadeBar());

            return spell;
        }

        private IEnumerator Progress(Spell spell)
        {
            float timePassed = Time.deltaTime;
            float rate = 1.0f / spell.CastTime;
            float progress = 0.0f;

            while (progress <= 1.0f)
            {
                castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
                progress += rate * Time.deltaTime;
                timePassed += Time.deltaTime;

                castTime.text = spell.CastTime - timePassed < 0 ? "0.00" : (spell.CastTime - timePassed).ToString("F2");

                yield return null;
            }

            StopCasting();
        }

        private IEnumerator FadeBar()
        {
            float rate = 1.0f / 0.25f;
            float progress = 0.0f;

            while (progress <= 1.0f)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
                progress += rate * Time.deltaTime;

                yield return null;
            }
        }

        public void StopCasting()
        {
            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
                canvasGroup.alpha = 0;
                fadeRoutine = null;
            }

            if (spellRoutine != null)
            {
                StopCoroutine(spellRoutine);
                spellRoutine = null;
            }
        }

        public Spell GetSpell(string spellName)
        {
            return Array.Find(spells, x => x.Name == spellName);
        }
    }
}