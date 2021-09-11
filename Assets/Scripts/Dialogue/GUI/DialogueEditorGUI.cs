using UnityEditor;
using UnityEngine;

namespace Dialogue.GUI
{
    public class DialogueEditorGUI : MonoBehaviour
    {
        private static GUISkin __gui;
        public static GUISkin gui{
            get{
                if(__gui == null){
                    if(EditorGUIUtility.isProSkin){
                        __gui = AssetDatabase.LoadAssetAtPath("Assets/Static Assets/Skins/EditorSkin.guiskin", typeof(GUISkin)) as GUISkin;
                    }else{
                        //__gui = AssetDatabase.LoadAssetAtPath("Assets/Dialoguer/DialogueEditor/Skins/dialogueEditorSkinLight.guiskin", typeof(GUISkin)) as GUISkin;
                    }
                }
                return __gui;
            }
        }

        private static Texture2D __borderTexture;

        public static Texture2D borderTexture
        {
            get
            {
                if (__borderTexture == null)
                    __borderTexture = AssetDatabase.LoadAssetAtPath("Assets/DialogueBorder.png", typeof(Texture2D)) as Texture2D;

                return __borderTexture;
            }
        }

        private static Texture2D __dialogueBack;

        public static Texture2D dialogueBack
        {
            get
            {
                if(__dialogueBack == null)
                    __dialogueBack = AssetDatabase.LoadAssetAtPath("Assets/EmptyPixel.png", typeof(Texture2D)) as Texture2D;

                return __dialogueBack;
            }
        }

        private static Texture2D __aquaButton;

        public static Texture2D aquaButton
        {
            get
            {
                if(__aquaButton == null)
                    __aquaButton = AssetDatabase.LoadAssetAtPath("Assets/Static Assets/Sprites/UI/GUI/Buttons/AquaButton.png", typeof(Texture2D)) as Texture2D;

                return __aquaButton;
            }
        }
        
        private static Texture2D __blueButton;

        public static Texture2D blueButton
        {
            get
            {
                if(__blueButton == null)
                    __blueButton = AssetDatabase.LoadAssetAtPath("Assets/Static Assets/Sprites/UI/GUI/Buttons/BlueButton.png", typeof(Texture2D)) as Texture2D;

                return __blueButton;
            }
        }
        
        private static Texture2D __greenButton;

        public static Texture2D greenButton
        {
            get
            {
                if(__greenButton == null)
                    __greenButton = AssetDatabase.LoadAssetAtPath("Assets/Static Assets/Sprites/UI/GUI/Buttons/GreenButton.png", typeof(Texture2D)) as Texture2D;

                return __greenButton;
            }
        }
        
        private static Texture2D __greyButton;

        public static Texture2D greyButton
        {
            get
            {
                if(__greyButton == null)
                    __greyButton = AssetDatabase.LoadAssetAtPath("Assets/Static Assets/Sprites/UI/GUI/Buttons/GreyButton.png", typeof(Texture2D)) as Texture2D;

                return __greyButton;
            }
        }
        
        private static Texture2D __orangeButton;

        public static Texture2D orangeButton
        {
            get
            {
                if(__orangeButton == null)
                    __orangeButton = AssetDatabase.LoadAssetAtPath("Assets/Static Assets/Sprites/UI/GUI/Buttons/OrangeButton.png", typeof(Texture2D)) as Texture2D;

                return __orangeButton;
            }
        }
        
        private static Texture2D __redButton;

        public static Texture2D redButton
        {
            get
            {
                if(__redButton == null)
                    __redButton = AssetDatabase.LoadAssetAtPath("Assets/Static Assets/Sprites/UI/GUI/Buttons/RedButton.png", typeof(Texture2D)) as Texture2D;

                return __redButton;
            }
        }
        
        private static Texture2D __yellowButton;

        public static Texture2D yellowButton
        {
            get
            {
                if(__yellowButton == null)
                    __yellowButton = AssetDatabase.LoadAssetAtPath("Assets/Static Assets/Sprites/UI/GUI/Buttons/YellowButton.png", typeof(Texture2D)) as Texture2D;

                return __yellowButton;
            }
        }
    }
}