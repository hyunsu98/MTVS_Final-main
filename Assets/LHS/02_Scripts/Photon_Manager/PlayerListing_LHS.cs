using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerListing_LHS : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    public Player Player { get; private set; }

    Texture2D picture;

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        _text.text = player.NickName;

        print("들어온 플레이어" + player.NickName);
        // 들어온 플레이어의 이미지를 요청한다
        // 저장된 텍스트랑 입력된 텍스트가 같으면 사진을 불러온다

        UserImage(player.NickName);
    }


    void UserImage(string userNick)
    {
        ChatAreaScript Area = GetComponent<ChatAreaScript>();

        Texture2D picture = Resources.Load<Texture2D>("ETC/" + userNick);

        // 사용자 이미지로 대체해서 넣는다 이건 내 PC에서만 하면 됨!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
