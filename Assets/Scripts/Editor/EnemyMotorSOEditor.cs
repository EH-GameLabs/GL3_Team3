using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyMotorData))]
public class EnemyMotorSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyMotorData script = (EnemyMotorData)target;

        // Disegna i campi normali
        script.movementSpeed = EditorGUILayout.FloatField("Movement Speed", script.movementSpeed);

        // Se il valore è true, mostra le variabili extra
        if (script.movementSpeed > 0)
        {
            script.enemyIdle = (EnemyBehaviour)EditorGUILayout.EnumPopup("Enemy Idle Behaviour", script.enemyIdle);
            script.enemyChasing = (EnemyBehaviour)EditorGUILayout.EnumPopup("Enemy Chasing Behaviour", script.enemyChasing);
        }

        // Salva le modifiche
        if (GUI.changed)
            EditorUtility.SetDirty(script);
    }
}
