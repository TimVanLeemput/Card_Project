//using UnityEditor;
//using UnityEditor.UIElements;
//using UnityEngine.UIElements;

//[CustomPropertyDrawer(typeof(CardInfo),true)]
//public class CardInfo_Editor_PropertyDrawer : PropertyDrawer
//{
//    public override VisualElement CreatePropertyGUI(SerializedProperty property)
//    {
//        VisualElement inspector = new VisualElement();

//        PropertyField cardResourceTypeField = new PropertyField(property.FindPropertyRelative("cardResourceType"));
//        PropertyField cardTitleField = new PropertyField(property.FindPropertyRelative("cardTitle"));
//        PropertyField cardFlavorTextField = new PropertyField(property.FindPropertyRelative("cardFlavorText"));
//        PropertyField allCardSkillsField = new PropertyField(property.FindPropertyRelative("allCardSkills"));
//        PropertyField cardArtSpriteField = new PropertyField(property.FindPropertyRelative("cardArtSprite"));
//        PropertyField cardArtMaterialField = new PropertyField(property.FindPropertyRelative("cardArtMaterial"));
//        PropertyField cardAttackField = new PropertyField(property.FindPropertyRelative("cardAttack"));
//        PropertyField cardHealthField = new PropertyField(property.FindPropertyRelative("cardHealth"));
//        PropertyField cardResourceCostField = new PropertyField(property.FindPropertyRelative("cardResourceCost"));
//        PropertyField cardTypeField = new PropertyField(property.FindPropertyRelative("cardType"));

//        inspector.Add(cardResourceTypeField);
//        inspector.Add(cardTitleField);
//        inspector.Add(cardFlavorTextField);
//        inspector.Add(allCardSkillsField);
//        inspector.Add(cardArtSpriteField);
//        inspector.Add(cardArtMaterialField);
//        inspector.Add(cardAttackField);
//        inspector.Add(cardHealthField);
//        inspector.Add(cardResourceCostField);
//        inspector.Add(cardTypeField);


//        return inspector;
//    }
//}
