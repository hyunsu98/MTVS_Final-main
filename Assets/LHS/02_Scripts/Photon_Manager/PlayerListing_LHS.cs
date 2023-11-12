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

        print("���� �÷��̾�" + player.NickName);
        // ���� �÷��̾��� �̹����� ��û�Ѵ�
        // ����� �ؽ�Ʈ�� �Էµ� �ؽ�Ʈ�� ������ ������ �ҷ��´�

        UserImage(player.NickName);
    }


    void UserImage(string userNick)
    {
        ChatAreaScript Area = GetComponent<ChatAreaScript>();

        Texture2D picture = Resources.Load<Texture2D>("ETC/" + userNick);

        // ����� �̹����� ��ü�ؼ� �ִ´� �̰� �� PC������ �ϸ� ��!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
