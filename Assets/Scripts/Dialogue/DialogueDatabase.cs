using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = nameof(DialogueDatabase), menuName = nameof(DialogueDatabase))]
    public class DialogueDatabase : ScriptableObject
    {
        [SerializeField] private string version;
        [SerializeField] private string author;
        [SerializeField] private string description;
        //private List<Character> character = new List<Character>();
        [SerializeField, HideInInspector] private List<Variable> variables = new List<Variable>();
        [SerializeField, HideInInspector] private List<Dialogue> dialogues = new List<Dialogue>();

        public List<Variable> Variables => variables;

        public List<Dialogue> Dialogues => dialogues;
    }
}