using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace TMPro
{
	[RequireComponent(typeof(TMP_Text))]
	[ExecuteInEditMode]
	public class TextMeshProEffect : MonoBehaviour
	{
		public enum EffectType : byte
		{
			Waves,
			Grow,
			Unfold,
			UnfoldAndWaves,
			Sketch,
			Bounce
		}

		public enum MixType : byte
		{
			Multiply,
			Add
		}

		[ExecuteInEditMode]
		internal class SharedState : MonoBehaviour
		{
			internal bool TextMeshIsUpdated;

			private void LateUpdate()
			{
				TextMeshIsUpdated = false;
			}
		}

		[Serializable]
		private sealed class öæ
		{
			public static readonly öæ öù = new öæ();

			public static Func<TextMeshProEffect, bool> öÿ;

			internal bool öÆ(TextMeshProEffect öû)
			{
				return öû == null || !öû.enabled;
			}
		}

		[StructLayout(LayoutKind.Auto)]
		private struct öÜ
		{
			public TMP_CharacterInfo öƒ;

			public TextMeshProEffect öá;
		}

		[StructLayout(LayoutKind.Auto)]
		private struct öí
		{
			public float öú;

			public TMP_CharacterInfo öñ;

			public TextMeshProEffect öÑ;
		}

		[StructLayout(LayoutKind.Auto)]
		private struct öª
		{
			public float òo;

			public TMP_CharacterInfo òο;

			public TextMeshProEffect òо;
		}

		public EffectType Type;

		public float DurationInSeconds = 0.5f;

		public float Amplitude = 1f;

		[Space]
		[Range(0f, 1f)]
		public float CharacterDurationRatio = 0f;

		public int CharactersPerDuration = 0;

		[Space]
		public Gradient Gradient = new Gradient();

		public MixType Mix = MixType.Multiply;

		[Space]
		public bool AutoPlay = true;

		public bool Repeat;

		public string ForWords;

		private readonly List<(int from, int to)> öé = new List<(int, int)>();

		[HideInInspector]
		public bool IsFinished;

		private float öâ;

		private TMP_Text öä;

		private EffectType öà;

		private bool öå;

		private bool öç;

		private bool öê;

		private float öë;

		private string öè;

		private ushort öï;

		private float[] öî = new float[10];

		private SharedState öì;

		private float öÄ;

		private TMP_TextInfo öÅ;

		private string öÉ;

		public List<(int from, int to)> Intervals => öé;

		private SharedState SharedStateProp
		{
			get
			{
				if (öì != null)
				{
					return öì;
				}
				öì = GetComponent<SharedState>();
				if (öì == null)
				{
					öì = base.gameObject.AddComponent<SharedState>();
					öì.hideFlags = (HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild);
				}
				return öì;
			}
		}

		public void CopyTo(TextMeshProEffect effect)
		{
			effect.Type = Type;
			effect.DurationInSeconds = DurationInSeconds;
			effect.Amplitude = Amplitude;
			effect.CharacterDurationRatio = CharacterDurationRatio;
			effect.CharactersPerDuration = CharactersPerDuration;
			effect.Gradient = Gradient;
			effect.Mix = Mix;
			effect.AutoPlay = AutoPlay;
			effect.Repeat = Repeat;
			effect.ForWords = ForWords;
		}

		public void Apply()
		{
			öä = GetComponent<TMP_Text>();
			öà = Type;
			öå = (öà == EffectType.Unfold || öà == EffectType.Grow || öà == EffectType.Bounce);
			öç = (öà == EffectType.Sketch);
			öê = false;
			öë = -1f;
		}

		private void OnEnable()
		{
			if (AutoPlay)
			{
				Play();
			}
		}

		private void OnDestroy()
		{
			öì = GetComponent<SharedState>();
			if (!(öì == null))
			{
				TextMeshProEffect[] components = base.gameObject.GetComponents<TextMeshProEffect>();
				if (components.Length == 0 || components.All(öæ.öù.öÆ))
				{
					UnityEngine.Object.Destroy(öì);
				}
			}
		}

		private void OnValidate()
		{
			if (AutoPlay)
			{
				Play();
			}
			else
			{
				Apply();
			}
		}

		private void LateUpdate()
		{
			if ((UnityEngine.Object)(object)öä == null || DurationInSeconds <= 0f || !öê)
			{
				return;
			}
			if (Repeat && IsFinished)
			{
				Play();
			}
			if (!SharedStateProp.TextMeshIsUpdated)
			{
				öä.ForceMeshUpdate();
				SharedStateProp.TextMeshIsUpdated = true;
			}
			öÅ = öä.textInfo;
			TMP_MeshInfo[] array = öÅ.CopyMeshInfoVertexData();
			int characterCount = öÅ.characterCount;
			if (characterCount == 0)
			{
				IsFinished = true;
				return;
			}
			float num = Time.realtimeSinceStartup - öâ;
			if (öè != öä.text || ForWords != öÉ)
			{
				öë = -1f;
				öè = öä.text;
				öÉ = ForWords;
				ë();
			}
			if (CharactersPerDuration > 0)
			{
				öÄ = DurationInSeconds * (float)öè.Length / (float)CharactersPerDuration;
			}
			else
			{
				öÄ = DurationInSeconds;
			}
			if (öç && num >= öë)
			{
				öë = num + öÄ;
				öï++;
				if (öî.Length < characterCount * 2)
				{
					öî = new float[characterCount * 2];
				}
				for (int i = 0; i < öî.Length; i++)
				{
					öî[i] = UnityEngine.Random.value;
				}
			}
			if (öå && num > öÄ)
			{
				num = öÄ;
				IsFinished = true;
			}
			float num2 = num / öÄ;
			if (!öå)
			{
				num2 %= 1f;
			}
			float characterDurationRatio = CharacterDurationRatio;
			float num3 = Mathf.Lerp(1f / (float)characterCount, 1f, characterDurationRatio);
			int num4 = 0;
			int num5 = characterCount;
			if (öé.Count > 0 || !string.IsNullOrEmpty(ForWords))
			{
				num5 = 0;
				for (int j = 0; j < öé.Count; j++)
				{
					(int, int) tuple = öé[j];
					num5 += tuple.Item2 - tuple.Item1 + 1;
				}
			}
			for (int k = 0; k < characterCount; k++)
			{
				if (öé.Count > 0 || !string.IsNullOrEmpty(ForWords))
				{
					bool flag = false;
					for (int l = 0; l < öé.Count; l++)
					{
						(int, int) tuple2 = öé[l];
						if (k >= tuple2.Item1 && k <= tuple2.Item2)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						continue;
					}
				}
				TMP_CharacterInfo î = öÅ.characterInfo[k];
				if (î.isVisible)
				{
					float num6 = Mathf.Lerp((float)num4 * 1f / (float)num5, 0f, characterDurationRatio);
					float value = (num2 - num6) / num3;
					value = Mathf.Clamp01(value);
					int materialReferenceIndex = î.materialReferenceIndex;
					int vertexIndex = î.vertexIndex;
					Color32[] colors = öÅ.meshInfo[materialReferenceIndex].colors32;
					Vector3[] vertices = array[materialReferenceIndex].vertices;
					Vector3[] vertices2 = öÅ.meshInfo[materialReferenceIndex].vertices;
					è(öÅ, î, vertexIndex, colors, vertices2, vertices, num2, value, öï);
					num4++;
				}
			}
			for (int m = 0; m < öÅ.meshInfo.Length; m++)
			{
				if (m < öÅ.materialCount)
				{
					öÅ.meshInfo[m].mesh.vertices = öÅ.meshInfo[m].vertices;
					öä.UpdateGeometry(öÅ.meshInfo[m].mesh, m);
				}
			}
			öä.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
		}

		private void ë()
		{
			öé.Clear();
			if (string.IsNullOrWhiteSpace(ForWords) || öè == null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder(öÅ.characterCount);
			for (int i = 0; i < öÅ.characterCount; i++)
			{
				stringBuilder.Append(öÅ.characterInfo[i].character);
			}
			bool flag = (öä.fontStyle & (FontStyles.LowerCase | FontStyles.UpperCase | FontStyles.SmallCaps)) != 0;
			string text = stringBuilder.ToString();
			string[] array = ForWords.Split(new char[2]
			{
				'\t',
				' '
			}, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				int startIndex = 0;
				while (true)
				{
					startIndex = text.IndexOf(text2, startIndex, flag ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
					if (startIndex == -1)
					{
						break;
					}
					öé.Add((startIndex, startIndex + text2.Length - 1));
					startIndex += text2.Length;
				}
			}
		}

		private void è(TMP_TextInfo ï, TMP_CharacterInfo î, int ì, Color32[] Ä, Vector3[] Å, Vector3[] É, float æ, float Æ, ushort û)
		{
			if (öç)
			{
				ª(î, ì, Ä, ù(öï + î.index));
			}
			else
			{
				ª(î, ì, Ä, Æ);
			}
			switch (Type)
			{
			case EffectType.Waves:
				οö(î, ì, Å, É, æ);
				break;
			case EffectType.Grow:
				οç(î, ì, Å, É, Æ);
				break;
			case EffectType.Unfold:
				οì(î, ì, Å, É, Æ);
				break;
			case EffectType.UnfoldAndWaves:
				οì(î, ì, Å, É, Æ);
				οö(î, ì, Å, Å, æ);
				break;
			case EffectType.Sketch:
				Ü(î, ì, Å, É, Æ, û);
				break;
			case EffectType.Bounce:
				οü(î, ì, Å, É, Æ);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private float ù(int ÿ)
		{
			int num = Mathf.Abs(ÿ % öî.Length);
			return öî[num];
		}

		private void Ü(TMP_CharacterInfo ƒ, int á, Vector3[] í, Vector3[] ú, float ñ, int Ñ)
		{
			öÜ οÜ = default(öÜ);
			οÜ.öƒ = ƒ;
			οÜ.öá = this;
			í[á] = ú[á] - οû(á, Ñ, ref οÜ);
			í[á + 1] = ú[á + 1] - οû(á + 1, Ñ, ref οÜ);
			í[á + 2] = ú[á + 2] - οû(á + 2, Ñ, ref οÜ);
			í[á + 3] = ú[á + 3] - οû(á + 3, Ñ, ref οÜ);
		}

		private void ª(TMP_CharacterInfo οo, int οο, Color32[] οо, float οô)
		{
			Color b = Gradient.Evaluate(οô);
			if (Mix == MixType.Multiply)
			{
				ref Color32 reference = ref οо[οο];
				reference *= b;
				ref Color32 reference2 = ref οо[οο + 1];
				reference2 *= b;
				ref Color32 reference3 = ref οо[οο + 2];
				reference3 *= b;
				ref Color32 reference4 = ref οо[οο + 3];
				reference4 *= b;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					Color c = οо[οο + i] + b;
					c.a *= b.a;
					οо[οο + i] = c;
				}
			}
		}

		private void οö(TMP_CharacterInfo οò, int οó, Vector3[] οÖ, Vector3[] οº, float οÇ)
		{
			öí οí = default(öí);
			οí.öú = οÇ;
			οí.öñ = οò;
			οí.öÑ = this;
			οÖ[οó] = οº[οó] - οƒ(οó, ref οí);
			οÖ[οó + 1] = οº[οó + 1] - οƒ(οó + 1, ref οí);
			οÖ[οó + 2] = οº[οó + 2] - οƒ(οó + 2, ref οí);
			οÖ[οó + 3] = οº[οó + 3] - οƒ(οó + 3, ref οí);
		}

		private void οü(TMP_CharacterInfo οé, int οâ, Vector3[] οä, Vector3[] οà, float οå)
		{
			öª οÑ = default(öª);
			οÑ.òo = οå;
			οÑ.òο = οé;
			οÑ.òо = this;
			οä[οâ] = οà[οâ] - οú(οâ, ref οÑ);
			οä[οâ + 1] = οà[οâ + 1] - οú(οâ + 1, ref οÑ);
			οä[οâ + 2] = οà[οâ + 2] - οú(οâ + 2, ref οÑ);
			οä[οâ + 3] = οà[οâ + 3] - οú(οâ + 3, ref οÑ);
		}

		private void οç(TMP_CharacterInfo οê, int οë, Vector3[] οè, Vector3[] οï, float οî)
		{
			οè[οë] = οï[οë];
			οè[οë + 3] = οï[οë + 3];
			οè[οë + 1] = Vector3.Lerp(οï[οë], οï[οë + 1], οî);
			οè[οë + 2] = Vector3.Lerp(οï[οë + 3], οï[οë + 2], οî);
			οè[οë] = Vector3.LerpUnclamped(οï[οë], οè[οë], Amplitude);
			οè[οë + 1] = Vector3.LerpUnclamped(οï[οë + 1], οè[οë + 1], Amplitude);
			οè[οë + 2] = Vector3.LerpUnclamped(οï[οë + 2], οè[οë + 2], Amplitude);
			οè[οë + 3] = Vector3.LerpUnclamped(οï[οë + 3], οè[οë + 3], Amplitude);
		}

		private void οì(TMP_CharacterInfo οÄ, int οÅ, Vector3[] οÉ, Vector3[] οæ, float οÆ)
		{
			Vector3 a = (οæ[οÅ] + οæ[οÅ + 1]) * 0.5f;
			Vector3 a2 = (οæ[οÅ + 3] + οæ[οÅ + 2]) * 0.5f;
			οÉ[οÅ] = Vector3.Lerp(a, οæ[οÅ], οÆ);
			οÉ[οÅ + 3] = Vector3.Lerp(a2, οæ[οÅ + 3], οÆ);
			οÉ[οÅ + 1] = Vector3.Lerp(a, οæ[οÅ + 1], οÆ);
			οÉ[οÅ + 2] = Vector3.Lerp(a2, οæ[οÅ + 2], οÆ);
			οÉ[οÅ] = Vector3.LerpUnclamped(οæ[οÅ], οÉ[οÅ], Amplitude);
			οÉ[οÅ + 1] = Vector3.LerpUnclamped(οæ[οÅ + 1], οÉ[οÅ + 1], Amplitude);
			οÉ[οÅ + 2] = Vector3.LerpUnclamped(οæ[οÅ + 2], οÉ[οÅ + 2], Amplitude);
			οÉ[οÅ + 3] = Vector3.LerpUnclamped(οæ[οÅ + 3], οÉ[οÅ + 3], Amplitude);
		}

		public void Play()
		{
			Apply();
			IsFinished = false;
			öâ = Time.realtimeSinceStartup;
			öê = true;
		}

		public void Finish()
		{
			öâ = 0f;
		}

		[CompilerGenerated]
		private Vector3 οû(int οù, int οÿ, ref öÜ οÜ)
		{
			float num = οÜ.öƒ.pointSize * 0.1f * Amplitude;
			float num2 = ù(οù << οÿ);
			float num3 = ù(οù << οÿ >> 5);
			return new Vector3(num2 * num, num3 * num, 0f);
		}

		[CompilerGenerated]
		private Vector3 οƒ(int οá, ref öí οí)
		{
			float f = (float)Math.PI * -2f * οí.öú + (float)(οá / 4) * 0.3f;
			return new Vector3(0f, Mathf.Cos(f) * οí.öñ.pointSize * 0.3f * Amplitude, 0f);
		}

		[CompilerGenerated]
		private Vector3 οú(int οñ, ref öª οÑ)
		{
			float f = (float)Math.PI * -2f * οÑ.òo;
			return new Vector3(0f, Mathf.Cos(f) * οÑ.òο.pointSize * 0.3f * Amplitude, 0f);
		}
	}
}
