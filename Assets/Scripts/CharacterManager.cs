using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] Vector2 generateRange;
    [SerializeField] int characterNumber;
    [SerializeField] float gridLength;

    public List<Character> characterList { get; private set; } = new List<Character>();
    public List<Character>[][] characterGirds;

    void Start()
    {
        for (int i = 0; i < characterNumber; i++)
        {
            // キャラクターをインスタンス化
            var obj = Instantiate(characterPrefab, transform);

            // 範囲内にランダムに配置
            var offset = new Vector3(
                generateRange.x * Random.Range(-1f, 1f), 0,
                generateRange.y * Random.Range(-1f, 1f));
            obj.transform.position = transform.position + offset;
            obj.transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);

            // 自身を登録
            var ch = obj.GetComponent<Character>();
            ch.manager = this;
            characterList.Add(ch);
        }

        // グリッドの作成
        characterGirds = new List<Character>[(int)(generateRange.x / gridLength * 2)][];
        for(int i = 0; i < characterGirds.Length; i++)
        {
            characterGirds[i] = new List<Character>[(int)(generateRange.y / gridLength * 2)];
            for(int j = 0; j < characterGirds[i].Length; j++)
            {
                characterGirds[i][j] = new List<Character>();
            }
        }
    }

    void Update()
    {
        // 各グリッドの登録をクリア
        for (int i = 0; i < characterGirds.Length; i++)
        {
            for (int j = 0; j < characterGirds[i].Length; j++)
            {
                characterGirds[i][j].Clear();
            }
        }

        // 全キャラクターをグリッドに登録
        foreach (var character in characterList)
        {
            GetGrid(character.transform.localPosition, out int gridX, out int gridY);
            characterGirds[gridX][gridY].Add(character);
        }
    }

    public void GetGrid(Vector3 localPosition, out int gridX, out int gridY)
    {
        float x = localPosition.x;
        float z = localPosition.z;
        gridX = (int)((x + generateRange.x) / gridLength);
        gridY = (int)((z + generateRange.y) / gridLength);
    }
}
