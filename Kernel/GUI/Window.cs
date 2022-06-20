#if HasGUI

using MOOS.FS;
using MOOS.Misc;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MOOS.GUI
{
    internal class Window
    {
        public static List<Window> Windows;
        public static IFont font;
        public static Image CloseButton;

        public static void Initialize()
        {
            Windows = new List<Window>();
            //PNG yehei = new PNG(File.Instance.ReadAllBytes("Images/M+.png"));
            PNG yehei = new PNG(File.Instance.ReadAllBytes("Images/Yahei.png"));
            CloseButton = new PNG(File.Instance.ReadAllBytes("Images/Close.png"));
            font = new IFont(yehei, "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~һ�Ҷ�ʮ�����߲��������ذ���ŵ��˵������������ڿ�����ʿ���´����������С��ɽ��ǧ���ڸ�Ϧ��ô�׷��輰������Ѿ��֮ʬ�����ȹ�����ҲŮ�з�ϰ����������������Ԫ����רؤ����ľ��֧����Ȯ̫����������ƥ�������͸�Ȼ�����ֹ��Ի���б�����ˮ����ţ����ë����ز����ʲƬ�ͻ�����Խ���צ���鸸���ؽ��׷ַ�����������Ƿ�絤���ڹ������Ŀ�����Ϊ����ƶ������߼��ĳ�����Ϳ׶Ӱ��������Ȱ˫�����δĩʾ���������˻ܰǹ���ȥ�������Žڱ����ɱ�����ʯ�Ҳ�������ƽ����������ռ͹¬ҵ��˧�鵩Ŀ��Ҷ���궣�������ֻ��ʷ����ߴ���ߵ߶��̾Ƚ��������ʸʧէ�����̴������ǰ�������Ϻ�������˦ӡ���־�Ҳ�î���⴦�������������������������֭��ͷ����Ѩ����д����ѵ���Ѷ����˾���񸥺������ū�ټ�Ƥ���з�ʥ��̨ì��ĸ��˿��ʽ�����ֶ����¼��ۿ����Ϲ���ִ��ɨ�س������â��֥���ӻ�Ȩ��������Э��ѹ�����ڰ��д��ҳ�����Ҵ������ɼ��Ĺ�аҢ������������ʦ�����ӹ⵱�������ų�������ͬ����������ߺ�����귫����������������ȶ�͢����Ǩ����ΰ��ƹ��������žʷ����ټ����˼��׷ݻ����»�α����Ѫ���ƺ�����ȫ��ɱ��������үɡ�����߶���ΣѮּ������������ɫ׳��ױ��ׯ�������뽻�´β������������ʴ��򲢹��׵��ݺ��۽�Ѵ������æ������լ�ְ����������������ũ����þ�Ѱ��Ѹ��������������ս��������績��������Ϸ��ۻ��������ѱԼ���ͳ���Ѳ��Ū������ν�����ԶΥ���˷���̳�������Ŷ������ַ���߳������ӹ�����ץ���հ���Т��������Ͷ�ؿӿ���������־��Ť���ѱ���ȴ���ܽ��έѿ���۽�ҲԷ���«о�Ϳ˰��ո˸ܶŲĴ�����ɼ�׼�������ϻ�����ᶹ������ҽ�������μ���������±��Ф������ʱ���������֨��Ż԰��Χѽ��������������Ա��������Ǻ�Ǵ��ؿ԰��غ�ڱ�˱᫸��ʲ��붤ĵ��������ͺ��˽ÿ����������ӵ����������Ӷ����סλ�������ŷ�ѽ����۷���ϣ�����׺��ڲ��θض��⳦�������̱���ɾ��ͮ�Ѿĵ���ӭ����ϵ�Զ�״Ķ�����������Ӧ����®������ұ�����м����ж�����Ƶ�������̭��ɳ�������ڷ���û��������߻��ǳ������κ��ξ�������֤������������ʶթ�ߺ��������鼴��ƨ��β�پָ��żɼ�½�����踽׹������梷���Ŭ�̾��Ӽ�γ����ɴ���ɲ��ݷ�ֽ�Ʒ�¿Ŧ���滷��������õ����Ĩ�Կ�����£��ƺ��̹����Ѻ��������Ķ�����ӵ�־��Ʊ����������Ұ�š��׾����������̧Ĵ����ȡ�Կ�������ïƻ��Ӣ��Է����ֱ���Ѿ�̦é����֦�����ö������ǹ�㹹��������ɥ�����´�����������������α����̬ŷŹ¢�޺���תն������������ϳ�Щ׿��²����������ζ�������������ǳ����������ϵ��������զ������ӽ���Ϳ��������������뿭���˷��Ṻ��ͼ����֪���մ�����ԹθѺͼ�ί����������ʹ�����İ�ֶ���²�ƾ��������������������������˾������ɲ���ȸ��ֲ�������̰��ƶ�޷���֫������ɰ����ʷ�в�ܻ����ú����������α��Ǳ侩���ӵ�ҹ������ű���μ��佼���Ͼ�ä�ſ���åբ��֣ȯ�����泴������¯ĭǳ��й����մ����Ͳ�����ע��Ţк��Ӿ����Ӳ����������Ӳ�����������ѧ���ڶ���������ٿ�����ʵ����ʫ�緿�ϳ�������������ѯ���꽨��¼������ӽ�ˢ�������ҳ���ªİ���½������ùý�����ʼķ�������μ���������ϸʻ֯����פ�������ﾭ�������ഺ��������ɺ�������ùҷ�ֿ��������Ю�������Ե�ק��ͦ����˩ʰ����ָ������ƴ�ڰ���Ų��ĳ�����׸�������ݼ�����ã���ٻ�ӫ�ʺ�������ҩ��ջ�̿ݱ�������դ��������������Ҫ����������ש�����ɰ���⿳����ˣǣŸ��������ѻ�Ծ±�ս��Ű������ʡ������������գ�ۺ�����ðӳ��������ηſθ����Ϻ��˼����Ʒ����ѫ���������ҧ������Ӵ̿Ͽ�������ݹ��ĸƶ۳��Ӹ���Կ�վ���ťж�װݿ���ձ������ѡ������������ظ��Ͷα�����˳���α��ٶ�������׷��Ż�Ȫ��������׷���ܴ������ɺ�������ʳ�����ʵ�ʤ������̥����ʨ����������óԹ������ʴ�ȱ����佫����ͤ���ȼ�ͥ�����߰�����������ʩ���������������������������¦ǰ����������ը˸���������ݽ�����⽽�Ƕ���ϴ����ǢȾ��䯼����޻�Ũ���Ѻ�ֻ�����ǡ�պ޾پ������ҹ���ͻ���Կͽ������������ף�����ջ�˵�п��˼�������ʺ�Ѷ�ѷü���ɳ���Ժ����������Ҧ��ŭ�ܺ�ӯ�µ��������ݰ��޽��ƽ����Ѥ������ʺ�ͳ���źİ���̩������������յ�����Բ��������ظ������Ӻ�����׽������Ԭ�ƶ����ż�컻��ֿ�ȿֵ���ͱ�����ܹ�������ç����Ī��ɻ����Өݺ�������ܵ�ͩ������˨�Ҹ�׮У�����������ٶ�����������贽������������ԭ��������ѳ�˽ν϶ٱ��²����Ǽ������ɹ��������Ѽ�β������ΰ�������������Կ�Ŷ���컽����󰡰������Ͷ��Բ������¸��Ǯǯ�������Ǧȱ����������˵г�������ȳ���͸��Ц��ծ��ֵ�а��㵹�Ⱦ㳫���޸����뽡���乪Ϣ��ͽ���󽢲հ㺽;���ʵ�Ҩ���򱪰������ȴ�֬�ظ����꽺��ŧ�������������ԧ���������������˥�Ը߹�ϯ׼��֢����ի����ƣ��Ч�����ƴ�����վ�ʾ������������߸�ƿȭ������濾�淳�������ӵ��������־������кƺ�Ϳԡ�������������˽�����ɬӿ����ĺ������ú������������խ���װ�������ŵ���ȷ������۱����ڤ˭��ԩ��׻̸�����չ��м���������������ˡ����ͨ����Ԥɣ������̿��������������´��������ڽ����ɵ�������Ʋ��������������ӵ��������̽�ݾ��ְ��������Ȣ�����ջƷ����ܾ�ή���Ѿ���Ƽ��өӪǬ������е������������÷������Ͱ��Ȳܸ�Ʊ�������ݹ�˶�ݿ�ˬ��Ϯʢ��ѩ����­���ȸ�ó����׳���������Ұž���������ķȾ�ֺ��Ծ�������߻��۶���������ΨơɶХ����ո�ߴ�ᡱ�����ӤȦ������ͭ�����������������Ʊ������Ϸ����������Ƴ�ż��͵����ͣƫ����������������̲�����б�и���Ϥ������Ų������������ݲ�����è�˲��ͼ��ڹݴռ�����������Ȭ�����ȿ�ӹ¹���¾������������ֲ�����Ǿ�ճ�����ϼ��޺����������������������ʻ컴��Ԩ�����Դ�Һ�ٵ������̺���������ϧ�ѵ�����Ω�������ҹ߿����ļ�����Ҥ��ı����г������ν���մ���ξ��������浰��¡���������ľ������������ά�����������׺���������������������߿�Խ�����������Ჩ��ϲ�����徾����Ԯ��ø��§��Ҽ��ɦ��˹��������ɢ����ļ���Ͼ��н����享������������Ҭֲɭ���ν��ù�׵�����ع����ֻݻ���ڼ����ֳ���Ӳ��ȷ����ֳ���ۼ��������̱�������Գ�������������������������������������������ϵ��������������Ѹ�Ⱦ�ι����������Ƕ��ñ���������������������������п���������ǵ�̺�ȶ�ʣ�Գ�ϡ˰�������ɸͲ������ݰ����Ʊ���������������½ֳ���ѭͧ���ⷬ������ƢҸǻ��³��⬻��ﱹȻ����װ���Ͷر�����ʹͯ�����������շ�������������ͺ���������ʪ�¿�������������������ָȷ߻Ŷ��㶻����修���Ԣ���ѽѴ������ԣ��ȹ��»лҥ��ǫϬ����ǿ�����϶��ý��ɩ���������¼��л�����ƭ��ɧԵɪ�����觻�����������İ�Я��ҡ����̯Ƹ������ѥ��ȵ��ĹĻ�������������״���������뻱��¥�����ҳ�а��Ɱ������µ���������������䶽Ƶ������������˯���ȱ�����ů��Ъ��Ͼ�ջ����������·�Ӹ�ǲ���϶��������ɤ����������ϴ�ê���സ׶�������̰����ɳ��ǳ��ǩ���پ����ɵ������΢��ң�����������������ȱ�Գӱ����ɷ��������������̵�������������������ܴ�ú����Į��Դ������Ϫ��������ݱ�������̲������į�����޽����㸣��Ⱥ�����ϱ���Ӽ޵�������ͽ˾�����׸��ǽ��δݺս��ľ�ժˤƲ��Ľĺġ�������ΰ���ε��ģ�����ե�Ÿ���Ϳ��������̼��Ը��Ͻշ���ѿų�����ӻ����Ӭ֩������׬�¶Ͷ�������Ѭ�������������ɮ������òĤ�������ɷ������ú����ڸ����������ý߶��쾫��Ǹ��Ϩ��ɿ������Ư��������©����կ���Ѳ�����̷�غ��������۴��ܵ���������˺����Ȥ�˳Ŵ��˲��ܶ�ײ����׫��Ь���������̺��ӣ�����Ϸ���Ʈ�״������ڰ�������ù���ⱩϹ��˻����Ӱ��̤���ٵ�����Ы�����������ī����������������¨��ƪ����Ƨ����ϥ��������Ħ������̱����������Ǳ�쳱̶����˳����ζ�㾰���������Ǵ�׺�ο����ԥ�Ժ��޲���������Ѧޱ��н���ߺ�ج����������ư�ѻ����޼���������������������Ĭǭ�����������������������ĥ�ȳ�������Ǹ�ȼ���輤����и���ڱ��ֽɴ�����ϲ�������̴����˪ϼ�t��˲ͫ������̣�����󰺿����κ�ɴط��վ�����������Ӯ�㿷��ų���α�����ź���ٸ�հ����������ӥ�ٽ�赴�����Ģ�����ضײ��������з����Ѣ����������ܰҫ�������Ρ����ħŴ��Ʃ����¶�������������ȿ�޴�", 18);
            MouseHandled = false;
        }

        public bool IsUnderMouse()
        {
            if (Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y && Control.MousePosition.Y < Y + Height) return true;
            return false;
        }

        public static void MoveToEnd(Window window)
        {
            Windows.Insert(0, window, true);
        }

        public static void DrawAll()
        {
            for (int i = Windows.Count - 1; i >= 0; i--)
            {
                if (Windows[i].Visible)
                    Windows[i].OnDraw();
            }
        }

        public static void InputAll()
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].Visible)
                    Windows[i].OnInput();
            }
        }

        public bool Visible 
        {
            set 
            {
                _visible = value;
                OnSetVisible(value);
            }
            get 
            {
                return _visible;
            }
        }

        public bool _visible;

        public static bool MouseHandled 
        {
            get => HasWindowMoving;
            set => HasWindowMoving = value;
        }

        public virtual void OnSetVisible(bool value) { }

        public int X, Y, Width, Height;

        public Window(int X, int Y, int Width, int Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Visible = true;
            Windows.Add(this);
#if Chinese
            Title = "����1";
#else
            Title = "Window1";
#endif
            MoveToEnd(this);
        }

        public int BarHeight = 40;
        public string Title;

        bool Move;
        int OffsetX;
        int OffsetY;
        public int Index { get => Windows.IndexOf(this); }

        public static bool HasWindowMoving = false;

        public virtual void OnInput()
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (
                    !HasWindowMoving &&
                    Control.MousePosition.X > CloseButtonX && Control.MousePosition.X < CloseButtonX + CloseButton.Width &&
                    Control.MousePosition.Y > CloseButtonY && Control.MousePosition.Y < CloseButtonY + CloseButton.Height
                )
                {
                    this.Visible = false;
                    return;
                }
                if (!HasWindowMoving && !Move && Control.MousePosition.X > X && Control.MousePosition.X < X + Width && Control.MousePosition.Y > Y - BarHeight && Control.MousePosition.Y < Y)
                {
                    MoveToEnd(this);
                    Move = true;
                    HasWindowMoving = true;
                    OffsetX = Control.MousePosition.X - X;
                    OffsetY = Control.MousePosition.Y - Y;
                }
            }
            else
            {
                Move = false;
                HasWindowMoving = false;
            }

            if (Move)
            {
                X = Control.MousePosition.X - OffsetX;
                Y = Control.MousePosition.Y - OffsetY;
            }
        }

        private int CloseButtonX => X + Width + 2 - (BarHeight / 2) - (CloseButton.Width / 2);
        private int CloseButtonY => Y - BarHeight + (BarHeight / 2) - (CloseButton.Height / 2);

        public virtual void OnDraw()
        {
            Framebuffer.Graphics.FillRectangle(X, Y - BarHeight, Width, BarHeight, 0xFF111111);
            //ASC16.DrawString(Title, X + ((Width/2)-((Title.Length*8)/2)), Y - BarHeight + (BarHeight / 4), 0xFFFFFFFF);

            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - ((BitFont.MeasureString("Song", Title)) / 2), Y - BarHeight + (BarHeight / 4));
            font.DrawString(X + (Width / 2) - ((font.MeasureString(Title)) / 2), Y - BarHeight + (BarHeight / 4), Title);
            //BitFont.DrawString("Song", 0xFFFFFFFF, Title, X + (Width / 2) - (BitFont.MeasureString("Song", Title) / 2), Y - BarHeight + (BarHeight / 4));

            Framebuffer.Graphics.FillRectangle(X, Y, Width, Height, 0xFF222222);
            DrawBorder();

            Framebuffer.Graphics.DrawImage(CloseButtonX, CloseButtonY, CloseButton);
        }

        public void DrawBorder(bool HasBar = true)
        {
            Framebuffer.Graphics.DrawRectangle(X - 1, Y - (HasBar ? BarHeight : 0) - 1, Width + 2, (HasBar ? BarHeight : 0) + Height + 2, 0xFF333333);
        }
    }
}
#endif