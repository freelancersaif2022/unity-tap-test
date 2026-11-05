# Unity Tap Test

A simple offline Unity Android game. Tap the moving target for 30 seconds and beat your saved best score. Touch/mouse controls, portrait safe-area layout, no ads, analytics or external assets.

## Build APK with GitHub Actions

[Open the Android workflow](https://github.com/freelancersaif2022/unity-tap-test/actions/workflows/android.yml). It starts on game changes to main and also supports Run workflow.

Unity activation is required. Add these in [Settings > Secrets and variables > Actions](https://github.com/freelancersaif2022/unity-tap-test/settings/secrets/actions):

| Secret | Value |
| --- | --- |
| UNITY_LICENSE | Contents of your activated Unity Personal .ulf license file |
| UNITY_EMAIL | Unity account email |
| UNITY_PASSWORD | Unity account password |

Unity Pro users can use UNITY_SERIAL instead of UNITY_LICENSE. Follow [GameCI's activation instructions](https://game.ci/docs/github/activation/). Enter credentials only in GitHub Secrets, never in source code or chat.

After adding secrets, rerun the failed job or click Run workflow. On success, download **UnityTapTest-APK** from the run's Artifacts section, extract the ZIP and install UnityTapTest.apk on Android 6.0 or later. GitHub sign-in is required. Artifacts expire after 14 days.

The output is a development APK with debug signing for testing. It is not a Play Store release. The workflow verifies Unity build success and a nonempty APK.

## Open locally

1. Install Unity **2022.3.40f1** with Android Build Support, SDK/NDK and OpenJDK.
2. Add the repository folder through Unity Hub.
3. Select **Tap Test > Open Demo Scene** and press Play.
4. Select **Tap Test > Build Android APK** to build locally.

The build script creates the scene and sets Android settings automatically. IL2CPP targets ARMv7 and ARM64. Output: build/Android/UnityTapTest.apk.

## Device checks

Start a round, tap targets, confirm the timer ends after 30 seconds, then replay. Beat your best score and relaunch to check persistence. Backgrounding ends the active round. Check portrait safe-area layout on your phone.

A committed project is not proof of a successful compile or device test. Check the workflow result for APK availability.
