using UnityEngine;
namespace UnityTapTest {
public sealed class TapGame : MonoBehaviour {
    enum Phase { Ready, Playing, Finished }
    Phase phase;
    int score, best;
    float remaining = 30;
    Rect target;
    GUIStyle title, label, button;
    readonly Color background = new Color(.04f, .06f, .11f);
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Launch() {
        if (FindObjectOfType<TapGame>() == null)
            new GameObject("Tap Game").AddComponent<TapGame>();
    }
    void Awake() {
        Application.targetFrameRate = 60;
        best = PlayerPrefs.GetInt("best", 0);
    }
    void Update() {
        if (phase != Phase.Playing) return;
        remaining = Mathf.Max(0, remaining - Time.unscaledDeltaTime);
        if (remaining <= 0) Finish();
    }
    void OnApplicationPause(bool paused) {
        if (paused && phase == Phase.Playing) Finish();
    }
    void Finish() {
        phase = Phase.Finished;
        if (score > best) {
            best = score;
            PlayerPrefs.SetInt("best", best);
            PlayerPrefs.Save();
        }
    }
    void StartRound() {
        score = 0; remaining = 30; phase = Phase.Playing; MoveTarget();
    }
    void MoveTarget() {
        float size = Mathf.Max(90, 130 - score);
        target = new Rect(Random.Range(30f, 510f - size),
            Random.Range(280f, 790f - size), size, size);
    }
    static void Fill(Rect rect, Color color) {
        Color old = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = old;
    }
    void OnGUI() {
        if (title == null) {
            title = new GUIStyle(GUI.skin.label) {
                fontSize = 43, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            label = new GUIStyle(title) { fontSize = 25, fontStyle = FontStyle.Normal };
            button = new GUIStyle(GUI.skin.button) { fontSize = 30, fontStyle = FontStyle.Bold };
        }
        Fill(new Rect(0, 0, Screen.width, Screen.height), background);
        Rect safe = Screen.safeArea;
        float scale = Mathf.Min(safe.width / 540, safe.height / 960);
        Matrix4x4 oldMatrix = GUI.matrix;
        GUI.matrix = Matrix4x4.TRS(new Vector3(
            safe.x + (safe.width - 540 * scale) / 2,
            Screen.height - safe.yMax + (safe.height - 960 * scale) / 2, 0),
            Quaternion.identity, new Vector3(scale, scale, 1));
        GUI.Label(new Rect(20, 55, 500, 75), "TAP THE TARGET", title);
        GUI.Label(new Rect(20, 140, 500, 45), "UNITY ANDROID TEST", label);
        Fill(new Rect(30, 210, 480, 4), new Color(.2f, .9f, .7f));
        if (phase == Phase.Playing) {
            GUI.Label(new Rect(20, 225, 250, 45), "Score: " + score, label);
            GUI.Label(new Rect(270, 225, 250, 45), "Time: " + Mathf.CeilToInt(remaining), label);
            Color oldBackground = GUI.backgroundColor;
            GUI.backgroundColor = new Color(.2f, 1, .75f);
            if (GUI.Button(target, "+1", button)) { score++; MoveTarget(); }
            GUI.backgroundColor = oldBackground;
            GUI.Label(new Rect(20, 825, 500, 45), "Tap the moving target!", label);
        } else {
            GUI.Label(new Rect(20, 320, 500, 70),
                phase == Phase.Ready ? "Ready to play?" : "Time is up!", title);
            GUI.Label(new Rect(20, 410, 500, 50),
                phase == Phase.Ready ? "You have 30 seconds" : "Your score: " + score, label);
            if (GUI.Button(new Rect(100, 540, 340, 90),
                phase == Phase.Ready ? "START GAME" : "PLAY AGAIN", button)) StartRound();
            GUI.Label(new Rect(20, 690, 500, 50), "Best score: " + best, label);
        }
        GUI.Label(new Rect(20, 900, 500, 40), "Offline  |  Touch or mouse", label);
        GUI.matrix = oldMatrix;
    }
}
}
