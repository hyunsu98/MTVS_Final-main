using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//편집모드에서도 스크립트의 각 이벤트 함수가 호출
[ExecuteInEditMode]

//여성 커스텀
public class FemaleTPPrefabMaker : MonoBehaviour

    {
    public bool allOptions;

    int hair;
    int chest;
    int legs;
    int feet;
    int tie;
    int jacket;

    //선택 시 활성화를 위한 변수
    //int skintone;
    public bool tieactive;
    public bool tieactivecolor;
    public bool glassesactive;
    public bool jacketactive;
    public bool hatactive;
    public bool legsactive;
    public bool haircoloractive;

    //머리 가져오기 위한 변수
    GameObject GOhead;
    GameObject GOheadsimple;

    //커스텀을 위한 변수
    GameObject[] GOfeet;
    GameObject[] GOhair;
    GameObject[] GOchest;
    GameObject[] GOlegs;
    GameObject GOglasses;
    GameObject[] GOjackets;
    GameObject[] GOties;

    //Material을 위한 
    //얼굴 (일반 / 노인)
    public Object[] MATSkins;
    public Object[] MATElderSkins;
    //머리카락 7개
    public Object[] MAThairA;
    public Object[] MAThairB;
    public Object[] MAThairC;
    public Object[] MAThairD;
    public Object[] MAThairE;
    public Object[] MAThairF;
    public Object[] MAThairG;
    //안경 (ROOT-Neck-Head-Glasses) (11개) Material
    public Object[] MATGlasses;
    //드레스 (2개)
    public Object[] MATDress;
    //티셔츠 (3개)
    public Object[] MATTshirt;
    //셔츠 (2개)
    public Object[] MATShirtA;
    //셔츠 (2개)
    public Object[] MATShirtB;
    //눈 (4개) Material
    public Object[] MATEyes;
    //자켓 2개
    public Object[] MATJacket;
    public Object[] MATSweater;
    //바지 
    public Object[] MATLegs;
    //신발 3개
    public Object[] MATFeetA;
    public Object[] MATFeetB;
    public Object[] MATFeetC;
    //모자 3개
    public Object[] MATHatA;
    public Object[] MATHatB;
    public Object[] MATHatC;
    //넥타이 (나비 / 기본(2개))
    public Object[] MATBowtie;
    public Object[] MATTie;
    //이빨
    public Object[] MATteeth;    

    //노인 변수 / 머리색
    public bool elder;
    Material headskin;

    void Start()
    {
        allOptions = false;

        //------Let's Get Ready ------
        //Menu();
        //Getready();
        //----------------------------
    }

    //준비를 위한 함수
    public void Getready()

    {
        //머리 가져오기
        GOhead = (GetComponent<Transform>().GetChild(0).gameObject);
        //머리 샘플 가져오고 비활성화 시키기 = BlendShapes없는 버전
        GOheadsimple = (GetComponent<Transform>().GetChild(1).gameObject);
        GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);

        //신발은 3개
        GOfeet = new GameObject[3];
        //머리카락은 7개 + 모자 3개
        GOhair = new GameObject[10];
        //상의는 9개
        GOchest = new GameObject[9];
        //하의는 4개
        GOlegs = new GameObject[4];
        //자켓은 2개
        GOjackets = new GameObject[2];
        //넥타이는 3개
        GOties = new GameObject[3];

        //Load models
        //신발 6 ~ 8 (3)
        for (int forAUX = 0; forAUX < 3; forAUX++) GOfeet[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 6).gameObject);
        //머리 9 ~ 18 (10)
        for (int forAUX = 0; forAUX < 10; forAUX++) GOhair[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 9).gameObject);
        //상의 총 9개
        //4 ~ 5 (드레스 2)
        for (int forAUX = 0; forAUX < 2; forAUX++) GOchest[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 4).gameObject);
        //22 ~ 25 (셔츠 4)
        for (int forAUX = 0; forAUX < 4; forAUX++) GOchest[forAUX+2] = (GetComponent<Transform>().GetChild(forAUX + 22).gameObject);
        //31 ~ 33 (티셔츠 3)
        for (int forAUX = 0; forAUX < 3; forAUX++) GOchest[forAUX+6] = (GetComponent<Transform>().GetChild(forAUX + 31).gameObject);
        //하의
        //19 ~ 21 (바지 3)
        for (int forAUX = 0; forAUX < 3; forAUX++) GOlegs[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 19).gameObject);
        //하의 4번째 - 26 (Skirt)
        GOlegs[3] = (GetComponent<Transform>().GetChild(26).gameObject);
        //자켓 27 ~ 28
        for (int forAUX = 0; forAUX < 2; forAUX++) GOjackets[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 27).gameObject);
        //넥타이
        //기본 29 ~ 30 (2개)
        for (int forAUX = 0; forAUX < 2; forAUX++) GOties[forAUX + 1] = (GetComponent<Transform>().GetChild(forAUX + 29).gameObject);
        //넥타이 0번째 - 3 (bowtie)
        GOties[0] = (GetComponent<Transform>().GetChild(3).gameObject); //bowtie 
        //안경 가져오기
        GOglasses = transform.Find("ROOT/TP/TP Pelvis/TP Spine/TP Spine1/TP Spine2/TP Neck/TP Head/Glasses").gameObject as GameObject;
        
        //만약 신발이 모두 활성화 되어있다면
        if (GOfeet[0].activeSelf && GOfeet[1].activeSelf && GOfeet[2].activeSelf)
        {
            ResetSkin();
            Randomize();
            elder = false;
            haircoloractive = true;
        }

        else
        {
            while (!GOhair[hair].activeSelf) hair++;if (hair > 6) hatactive = true;
            else hatactive = false;
            while (!GOchest[chest].activeSelf) chest++;
            if (chest != 0) while (!GOlegs[legs].activeSelf) legs++;
            while (!GOfeet[feet].activeSelf) feet++;
            if (GOjackets[0].activeSelf) jacket = 0; if (GOjackets[1].activeSelf) jacket = 1;
            if (!GOjackets[0].activeSelf && !GOjackets[1].activeSelf) jacket = 2;
            tie = 3;
            for (int forAUX = 0; forAUX > 3; forAUX++)
            {
                if (GOties[forAUX].activeSelf) tie = forAUX;
            }
            if (!GOties[0].activeSelf && !GOties[1].activeSelf && !GOties[2].activeSelf) { tieactive = false; tieactivecolor = false; }
            if (GOglasses.activeSelf) glassesactive = true;
            Checkties();
            Checklegs();
            Checkelder();
        }
    }

    //(피부 / 상의 / 하의 색)
    void ResetSkin()
    {
        //피부 Material (일반 / 노인 4개씩)
        string[] allskins = new string[8] { "TPFemaleA0", "TPFemaleB0", "TPFemaleC0", "TPFemaleD0", "TP_E_FemaleA0", "TP_E_FemaleB0", "TP_E_FemaleC0", "TP_E_FemaleD0" };
        
        Material[] AUXmaterials;
        int materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;

        //피부 색
        //ref head material
        AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
        materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;

        for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
            for (int forAUX3 = 0; forAUX3 < allskins.Length; forAUX3++)
                for (int forAUX4 = 1; forAUX4 < MATSkins.Length+1; forAUX4++)
                {
                    if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                    {
                        headskin = AUXmaterials[forAUX2];
                    }
                }
        
        //하의 색
        //legs
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++)
        {
            AUXmaterials = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials.Length;

            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < 4; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }
        }
        
        //상의 색
        //chest
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++)
        {
            AUXmaterials = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials.Length;

            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < 4; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOchest[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }
        }
    }
    //모두 비활성화
    public void Deactivateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOjackets.Length; forAUX++) GOjackets[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOties.Length; forAUX++) GOties[forAUX].SetActive(false);
        GOglasses.SetActive(false);
        glassesactive = false;
        jacketactive = false;
        tieactivecolor = false;
        tieactive = false;
        tieactivecolor = false;
        hatactive = false;
    }
    //모두 활성화
    public void Activateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOjackets.Length; forAUX++) GOjackets[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOties.Length; forAUX++) GOties[forAUX].SetActive(true);
        GOglasses.SetActive(true);
    }
    //모든 옵션
    public void Menu()
    {
        allOptions = !allOptions;
    }

    //하의 선택
    public void Checklegs()
    {
        //상의가 0과 같다면
        if (chest ==0)
        {
            //하의 비활성화
            legsactive = false;
            GOlegs[legs].SetActive(false);
        }
        //그렇지 않다면
        else
        {
            //하의 활성화
            legsactive = true;
            GOlegs[legs].SetActive(true);
        }
    }
    //넥타이 선택
    public void Checkties()
    {
        //상의 2, 3과 같다면
        if (chest ==2 || chest ==3)
        {
            //넥타이 활성화
            tieactive = true;
            //만약 3이 아니라면
            if (tie != 3)
            {
                //넥타이 활성화
                GOties[tie].SetActive(true);
                //색도 켜지기
                tieactivecolor = true;
            }
            // 그렇지 않으면 비활성화
            else tieactivecolor = false;
        }
        //나머지 상의라면
        else
        {
            //만약 3이 아니라면 넥타이 비활성화
            if (tie != 3) GOties[tie].SetActive(false);
            tieactive = false;
            tieactivecolor = false;
        }
    }
    //노인 선택
    void Checkelder()
    {
        Material[] AUXmaterials;
        elder = false;
        haircoloractive = true;

        AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
        int materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;

        for (int forAUX = 0; forAUX < materialcount; forAUX++)
        {
            if (AUXmaterials[forAUX].name == MATteeth[1].name)
            {
                elder = true;
                haircoloractive = false;
            }            
        }
    }

    //models
    //모자
    public void Nexthat()
    {
        hatactive = true;
        // 일반 머리일때는
        if (hair < 7)
        {
            //머리 비활성화 후 7번째 머리로 활성화 (모자는 7번째 머리만 가능)
            GOhair[hair].SetActive(false);
            hair = 7;
            GOhair[hair].SetActive(true);
        }
        // 모자쓴 머리일 때는
        else
        {
            GOhair[hair].SetActive(false);
            hair++;
            //hair 반복
            if (hair > GOhair.Length-1) hair = 7;          
            GOhair[hair].SetActive(true);
        }
    }
    public void Prevhat()
    {
        hatactive = true;
        if (hair < 7)
        {
            GOhair[hair].SetActive(false);
            hair = 9;
            GOhair[hair].SetActive(true);
        }
        else
        {
            GOhair[hair].SetActive(false);
            hair--;
            if (hair < 7) hair = 9;
            GOhair[hair].SetActive(true);
        }
    }

    public void Nexthair()
    {
        hatactive = false;
        GOhair[hair].SetActive(false);
        
        //모자 길이를 빼줘야함
        if (hair < GOhair.Length - 4) hair++;
        else hair = 0;
        GOhair[hair].SetActive(true);
    }
    public void GlassesOn()
    {
        glassesactive = !glassesactive;
        GOglasses.SetActive(glassesactive);
    }

    //상의 (가능한 넥타이 / 하의가 있음)
    public void Nextchest()
    {
        GOchest[chest].SetActive(false);
        if (chest < GOchest.Length - 1) chest++;
        else chest = 0;
        GOchest[chest].SetActive(true);

        //넥타이 선택
        Checkties();
        //하의 선택
        Checklegs();
    }
    public void Nextlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs < GOlegs.Length - 1) legs++;
        else legs = 0;
        GOlegs[legs].SetActive(true);
    }
    public void Nextfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet < GOfeet.Length - 1) feet++;
        else feet = 0;
        GOfeet[feet].SetActive(true);
    }
    public void Nexttie()
    {
        if (tie != 3) GOties[tie].SetActive(false);
        if (tie < GOties.Length) tie++;
        else tie = 0;
        if (tie != 3) GOties[tie].SetActive(true);

        // material
        if (tie == 3) tieactivecolor = false;
        else tieactivecolor = true;
    }
    public void Nextjacket()
    {
        if (jacket == 2)
        {
            jacket = 0;
            GOjackets[jacket].SetActive(true);
            jacketactive = true;
        }
        else
        {
            //반복
            if (jacket == 1)
            {
                GOjackets[jacket].SetActive(false);
                jacket = 2;
                jacketactive = false;
            }

            if (jacket == 0)
            {
                GOjackets[jacket].SetActive(false);
                jacket = 1;
                GOjackets[jacket].SetActive(true);
            }
        }
    }

    public void Prevhair()
    {
        hatactive = false;
        GOhair[hair].SetActive(false);
        if (hair > 0) hair--;
        else hair = 6;
        GOhair[hair].SetActive(true);
    }
    public void Prevchest()
    {
        GOchest[chest].SetActive(false);
        chest--;
        if (chest < 0) chest = GOchest.Length - 1;
        GOchest[chest].SetActive(true);
        Checkties();
        Checklegs();
    }
    public void Prevlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs > 0) legs--;
        else legs = GOlegs.Length - 1;
        GOlegs[legs].SetActive(true);
    }
    public void Prevfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet > 0) feet--;
        else feet = GOfeet.Length - 1;
        GOfeet[feet].SetActive(true);
    }
    public void Prevtie()
    {
        if (tie != 3) GOties[tie].SetActive(false);
        tie--;
        if (tie < 0) tie = 3;
        if (tie != 3) GOties[tie].SetActive(true);
        if (tie == 3) tieactivecolor = false;
        else tieactivecolor = true;
    }
    public void Prevjacket()
    {
        if (jacket == 0)
        {
            GOjackets[jacket].SetActive(false);
            jacket = 2;
            jacketactive = false;
        }
        else
        {
            if (jacket == 1)
            {
                GOjackets[jacket].SetActive(false);
                jacket = 0;
                GOjackets[jacket].SetActive(true);
            }
            if (jacket == 2)
            {
                jacket = 1;
                jacketactive = true;
                GOjackets[jacket].SetActive(true);
            }
        }
    }
    
    //materials    
    public void Nexthatcolor(int todo)
    {
        if (hatactive)
        {
            if (hair == 7) ChangeMaterials(MATHatA, todo);
            if (hair == 8) ChangeMaterials(MATHatB, todo);
            if (hair == 9) ChangeMaterials(MATHatC, todo);
        }
    }
    public void Nextskincolor(int todo)
    {
        ChangeMaterials(MATSkins, todo);
        ChangeMaterials(MATElderSkins, todo);
    }
    public void Nexthaircolor(int todo)
    {
        if (!elder)
        {
            int intindex = 0;
            Material AUXmaterial;
            AUXmaterial = GOhair[0].GetComponent<Renderer>().sharedMaterial;
            while (AUXmaterial.name != MAThairA[intindex].name) intindex++;
            if (intindex == 2 && todo == 0) todo = 3;
            if (intindex == 0 && todo == 1) todo = 4;
            ChangeMaterials(MAThairA, todo);
            ChangeMaterials(MAThairB, todo);
            ChangeMaterials(MAThairC, todo);
            ChangeMaterials(MAThairD, todo);
            ChangeMaterials(MAThairE, todo);
            ChangeMaterials(MAThairF, todo);
            ChangeMaterials(MAThairG, todo);
        }
    }
    public void Nextglasses(int todo)
    {
        ChangeMaterials(MATGlasses, todo);
    }
    public void Nexteyescolor(int todo)
    {
        ChangeMaterials(MATEyes, todo);
    }
    public void Nextchestcolor(int todo)
    {
        if (chest < 2) ChangeMaterial(GOchest[0], MATDress, todo);
        if (chest < 2) ChangeMaterial(GOchest[1], MATDress, todo);
        if (chest ==2 || chest == 3) ChangeMaterials(MATShirtA, todo);
        if (chest == 4 || chest == 5) ChangeMaterials(MATShirtB, todo); 
        if (chest > 5) ChangeMaterials(MATTshirt, todo); 
    }
    public void Nextjacketcolor(int todo)
    {
        if (jacket == 0) ChangeMaterials(MATJacket, todo);
        if (jacket == 1) ChangeMaterials(MATSweater, todo);
    }
    public void Nextlegscolor(int todo)
    {
        ChangeMaterials(MATLegs, todo);
        ChangeMaterial(GOlegs[3], MATDress, todo);
    }
    public void Nextfeetcolor(int todo)
    {
        if (feet == 0) ChangeMaterials(MATFeetA, todo);
        if (feet == 1) ChangeMaterials(MATFeetB, todo);
        if (feet == 2) ChangeMaterials(MATFeetC, todo);
    }
    public void Nexttiecolor(int todo)
    {
        if (tie == 0) ChangeMaterials(MATBowtie, todo);
        if (tie > 0) ChangeMaterials(MATTie, todo);
    }
         

    public void ResetModel()
    {
            ElderOff();
            Activateall();
            ChangeMaterials(MATHatA, 3);
            ChangeMaterials(MATHatB, 3);
            ChangeMaterials(MATHatC, 3);
            ChangeMaterials(MATSkins, 3);
            ChangeMaterials(MAThairA, 3);
            ChangeMaterials(MAThairB, 3);
            ChangeMaterials(MAThairC, 3);
            ChangeMaterials(MAThairD, 3);
            ChangeMaterials(MAThairE, 3);
            ChangeMaterials(MAThairF, 3);
            ChangeMaterials(MAThairG, 3);
            ChangeMaterials(MATGlasses, 3);
            ChangeMaterials(MATEyes, 3);
            ChangeMaterials(MATShirtA, 3);
            ChangeMaterials(MATShirtB, 3);
            ChangeMaterials(MATTshirt, 3);
            ChangeMaterials(MATDress, 3);
            ChangeMaterials(MATJacket, 3);
            ChangeMaterials(MATSweater, 3);
            ChangeMaterials(MATLegs, 3);
            ChangeMaterials(MATFeetA, 3);
            ChangeMaterials(MATFeetB, 3);
            ChangeMaterials(MATBowtie, 3);
            ChangeMaterials(MATTie, 3);
            ChangeMaterials(MATteeth, 3);
            Menu();
    }
    public void Randomize()
    {
        Deactivateall();
        ResetSkin();
        Checkelder();
        hair = Random.Range(0, 15);
        if (hair > 9) hair = Random.Range(0, 5);
        GOhair[hair].SetActive(true);
        if (hair > 5) hatactive = true;
        chest = Random.Range(0, GOchest.Length); GOchest[chest].SetActive(true);
        tie = Random.Range(0, 4);
        Checkties();
        legs = Random.Range(0, GOlegs.Length); GOlegs[legs].SetActive(true);
        Checklegs();
        feet = Random.Range(0, 2); GOfeet[feet].SetActive(true);
        jacket = Random.Range(0, 3);
        if (jacket < 2)
        {
            jacketactive = true;
            GOjackets[jacket].SetActive(true);
        }
        else jacketactive = false;
        if (Random.Range(0, 4) > 2)
        {
            glassesactive = true;
            GOglasses.SetActive(true);
            ChangeMaterial(GOglasses, MATGlasses, 2);
        }
        else glassesactive = false;

        //materials
        ChangeMaterial(GOhead, MATEyes, 2);
        if (tieactivecolor) for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 10)); forAUX2++) Nexttiecolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 8)); forAUX2++) Nexthaircolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 32)); forAUX2++) Nextskincolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 26)); forAUX2++) Nextfeetcolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 26)); forAUX2++) Nextjacketcolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 24)); forAUX2++) Nexthatcolor(0);
        if (legsactive) for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 32)); forAUX2++) Nextlegscolor(0);
        
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 34)); forAUX2++) Nextchestcolor(0);
    }
    public void CreateCopy()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 33; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TP/TP Pelvis/TP Spine/TP Spine1/TP Spine2/TP Neck/TP Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<FemaleTPPrefabMaker>());
    }
    public void FIX()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 33; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TP/TP Pelvis/TP Spine/TP Spine1/TP Spine2/TP Neck/TP Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<FemaleTPPrefabMaker>());
        DestroyImmediate(gameObject);
    }

    //노인
    public void ElderOn()
    {
        elder = true;
        haircoloractive = false;
        //blendshapes
        SkinnedMeshRenderer rendhead;
        rendhead = GOhead.GetComponent<SkinnedMeshRenderer>();
        rendhead.SetBlendShapeWeight(26, 100);
        

        //skin        
        SwitchMaterial(GOhead, MATSkins, MATElderSkins);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) SwitchMaterial(GOchest[forAUX], MATSkins, MATElderSkins);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) SwitchMaterial(GOlegs[forAUX], MATSkins, MATElderSkins);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) SwitchMaterial(GOfeet[forAUX], MATSkins, MATElderSkins);
        
        //teeth
        ChangeMaterials(MATteeth, 1);

        //hair
        ChangeMaterials(MAThairA, 5);
        ChangeMaterials(MAThairB, 5);
        ChangeMaterials(MAThairC, 5);
        ChangeMaterials(MAThairD, 5);
        ChangeMaterials(MAThairE, 5);
        ChangeMaterials(MAThairF, 5);
        ChangeMaterials(MAThairG, 5);

    }
    public void ElderOff()

    {
        elder = false;
        haircoloractive = true;
        //blendshapes
        SkinnedMeshRenderer rendhead;
        rendhead = GOhead.GetComponent<SkinnedMeshRenderer>();
        rendhead.SetBlendShapeWeight(26, 0);
        

        //skin
        SwitchMaterial(GOhead, MATElderSkins, MATSkins);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) SwitchMaterial(GOchest[forAUX], MATElderSkins, MATSkins);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) SwitchMaterial(GOlegs[forAUX], MATElderSkins, MATSkins);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) SwitchMaterial(GOfeet[forAUX], MATElderSkins, MATSkins);

        //teeth
        ChangeMaterials(MATteeth, 1);

        //hair 
        ChangeMaterials(MAThairA, 3);
        ChangeMaterials(MAThairB, 3);
        ChangeMaterials(MAThairC, 3);
        ChangeMaterials(MAThairD, 3);
        ChangeMaterials(MAThairE, 3);
        ChangeMaterials(MAThairF, 3);
        ChangeMaterials(MAThairG, 3);
    }

    void ChangeMaterial(GameObject GO, Object[] MAT, int todo)
    {
        bool found = false;
        int MATindex = 0;
        int subMAT = 0;
        Material[] AUXmaterials;
        AUXmaterials = GO.GetComponent<Renderer>().sharedMaterials;
        int materialcount = GO.GetComponent<Renderer>().sharedMaterials.Length;

        for (int forAUX = 0; forAUX < materialcount; forAUX++)
            for (int forAUX2 = 0; forAUX2 < MAT.Length; forAUX2++)
            {
                if (AUXmaterials[forAUX].name == MAT[forAUX2].name)
                {
                    subMAT = forAUX;
                    MATindex = forAUX2;
                    found = true;
                }
            }
        if (found)
        {
            if (todo == 0) //increase
            {
                MATindex++;
                if (MATindex > MAT.Length - 1) MATindex = 0;
            }
            if (todo == 1) //decrease
            {
                MATindex--;
                if (MATindex < 0) MATindex = MAT.Length - 1;
            }
            if (todo == 2) //random value
            {
                MATindex = Random.Range(0, MAT.Length);
            }
            if (todo == 3) //reset value
            {
                MATindex = 0;
            }
            if (todo == 4) //penultimate
            {
                MATindex = MAT.Length - 2;
            }
            if (todo == 5) //last one
            {
                MATindex = MAT.Length - 1;
            }
            AUXmaterials[subMAT] = MAT[MATindex] as Material;
            GO.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        }
    }

    //노인 색
    void ChangeMaterials(Object[] MAT, int todo)
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) ChangeMaterial(GOhair[forAUX], MAT, todo);
        ChangeMaterial(GOhead, MAT, todo);
        ChangeMaterial(GOglasses, MAT, todo);

        ChangeMaterial(GOheadsimple, MAT, todo);
        
        ChangeMaterial(GOjackets[0], MAT, todo);
        ChangeMaterial(GOjackets[1], MAT, todo);
        for (int forAUX = 0; forAUX < GOties.Length; forAUX++) ChangeMaterial(GOties[forAUX], MAT, todo);

        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) ChangeMaterial(GOchest[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) ChangeMaterial(GOlegs[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) ChangeMaterial(GOfeet[forAUX], MAT, todo);

    }
    void SwitchMaterial(GameObject GO, Object[] MAT1, Object[] MAT2)
    {
        Material[] AUXmaterials;
        AUXmaterials = GO.GetComponent<Renderer>().sharedMaterials;
        int materialcount = GO.GetComponent<Renderer>().sharedMaterials.Length;
        int index = 0;
        for (int forAUX = 0; forAUX < materialcount; forAUX++)
            for (int forAUX2 = 0; forAUX2 < MAT1.Length; forAUX2++)
            {
                if (AUXmaterials[forAUX] == MAT1[forAUX2])
                {
                    index = forAUX2;
                    if (forAUX2 > MAT2.Length-1) index -= (int)Mathf.Floor(index / 4)*4;
                    AUXmaterials[forAUX] = MAT2[index] as Material;
                    GO.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                }
            }
    }


    #region Test 출력 함수. (EditorFemalePrefabMaker 에서 부를거임)

    public void Print_Test(int num)
    {
        for (int i = 0; i < num; ++i)
        { 
            print("Test 하는 중");
        }
    }

    #endregion
}


