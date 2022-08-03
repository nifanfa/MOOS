﻿#if Kernel
using MOOS.FS;
using MOOS.Misc;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;

namespace System.Windows
{
    public static class WindowManager
    {
        public static List<Window> Childrens;
        public static IFont font;
        public static Image CloseButton;


        public static void Initialize()
        {
            Childrens = new List<Window>();
            //PNG yehei = new PNG(File.Instance.ReadAllBytes("sys/media/M+.png"));
            PNG yehei = new PNG(File.Instance.ReadAllBytes("sys/media/Yahei.png"));
            CloseButton = new PNG(File.Instance.ReadAllBytes("sys/media/Close.png"));
            //ASCII+一级汉字
            font = new IFont(yehei, "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~一乙二十丁厂七卜八人入儿匕几九刁了刀力乃又三干于亏工土士才下寸大丈与万上小口山巾千乞川亿个夕久么勺凡丸及广亡门丫义之尸己已巳弓子卫也女刃飞习叉马乡丰王开井天夫元无云专丐扎艺木五支厅不犬太区历歹友尤匹车巨牙屯戈比互切瓦止少曰日中贝冈内水见午牛手气毛壬升夭长仁什片仆化仇币仍仅斤爪反介父从仑今凶分乏公仓月氏勿欠风丹匀乌勾凤六文亢方火为斗忆计订户认冗讥心尺引丑巴孔队办以允予邓劝双书幻玉刊未末示击打巧正扑卉扒功扔去甘世艾古节本术可丙左厉石右布夯戊龙平灭轧东卡北占凸卢业旧帅归旦目且叶甲申叮电号田由只叭史央兄叽叼叫叩叨另叹冉皿凹囚四生矢失乍禾丘付仗代仙们仪白仔他斥瓜乎丛令用甩印尔乐句匆册卯犯外处冬鸟务包饥主市立冯玄闪兰半汁汇头汉宁穴它讨写让礼训议必讯记永司尼民弗弘出辽奶奴召加皮边孕发圣对台矛纠母幼丝邦式迂刑戎动扛寺吉扣考托老巩圾执扩扫地场扬耳芋共芒亚芝朽朴机权过臣吏再协西压厌戌在百有存而页匠夸夺灰达列死成夹夷轨邪尧划迈毕至此贞师尘尖劣光当早吁吐吓虫曲团吕同吊吃因吸吗吆屿屹岁帆回岂则刚网肉年朱先丢廷舌竹迁乔迄伟传乒乓休伍伏优臼伐延仲件任伤价伦份华仰仿伙伪自伊血向似后行舟全会杀合兆企众爷伞创肌肋朵杂危旬旨旭负匈名各多争色壮冲妆冰庄庆亦刘齐交衣次产决亥充妄闭问闯羊并关米灯州汗污江汛池汝汤忙兴宇守宅字安讲讳军讶许讹论讼农讽设访诀寻那迅尽导异弛孙阵阳收阶阴防奸如妇妃好她妈戏羽观欢买红驮纤驯约级纪驰纫巡寿弄麦玖玛形进戒吞远违韧运扶抚坛技坏抠扰扼拒找批址扯走抄贡汞坝攻赤折抓扳抡扮抢孝坎均抑抛投坟坑抗坊抖护壳志块扭声把报拟却抒劫芙芜苇芽花芹芥芬苍芳严芦芯劳克芭苏杆杠杜材村杖杏杉巫极李杨求甫匣更束吾豆两酉丽医辰励否还尬歼来连轩步卤坚肖旱盯呈时吴助县里呆吱吠呕园旷围呀吨足邮男困吵串员呐听吟吩呛吻吹呜吭吧邑吼囤别吮岖岗帐财针钉牡告我乱利秃秀私每兵估体何佐佑但伸佃作伯伶佣低你住位伴身皂伺佛囱近彻役返余希坐谷妥含邻岔肝肛肚肘肠龟甸免狂犹狈角删条彤卵灸岛刨迎饭饮系言冻状亩况床库庇疗吝应这冷庐序辛弃冶忘闰闲间闷判兑灶灿灼弟汪沐沛汰沥沙汽沃沦汹泛沧没沟沪沈沉沁怀忧忱快完宋宏牢究穷灾良证启评补初社祀识诈诉罕诊词译君灵即层屁尿尾迟局改张忌际陆阿陈阻附坠妓妙妖姊妨妒努忍劲矣鸡纬驱纯纱纲纳驳纵纷纸纹纺驴纽奉玩环武青责现玫表规抹卦坷坯拓拢拔坪拣坦担坤押抽拐拖者拍顶拆拎拥抵拘势抱拄垃拉拦幸拌拧拂拙招坡披拨择抬拇拗其取茉苦昔苛若茂苹苗英苟苑苞范直茁茄茎苔茅枉林枝杯枢柜枚析板松枪枫构杭杰述枕丧或画卧事刺枣雨卖郁矾矿码厕奈奔奇奋态欧殴垄妻轰顷转斩轮软到非叔歧肯齿些卓虎虏肾贤尚旺具味果昆国哎咕昌呵畅明易咙昂迪典固忠呻咒咋咐呼鸣咏呢咄咖岸岩帖罗帜帕岭凯败账贩贬购贮图钓制知迭氛垂牧物乖刮秆和季委秉佳侍岳供使例侠侥版侄侦侣侧凭侨佩货侈依卑的迫质欣征往爬彼径所舍金刹命肴斧爸采觅受乳贪念贫忿肤肺肢肿胀朋股肮肪肥服胁周昏鱼兔狐忽狗狞备饰饱饲变京享庞店夜庙府底疟疙疚剂卒郊庚废净盲放刻育氓闸闹郑券卷单炬炒炊炕炎炉沫浅法泄沽河沾泪沮油泊沿泡注泣泞泻泌泳泥沸沼波泼泽治怔怯怖性怕怜怪怡学宝宗定宠宜审宙官空帘宛实试郎诗肩房诚衬衫视祈话诞诡询该详建肃录隶帚屉居届刷屈弧弥弦承孟陋陌孤陕降函限妹姑姐姓妮始姆迢驾叁参艰线练组绅细驶织驹终驻绊驼绍绎经贯契贰奏春帮玷珍玲珊玻毒型拭挂封持拷拱项垮挎城挟挠政赴赵挡拽哉挺括垢拴拾挑垛指垫挣挤拼挖按挥挪拯某甚荆茸革茬荐巷带草茧茵茶荒茫荡荣荤荧故胡荫荔南药标栈柑枯柄栋相查柏栅柳柱柿栏柠树勃要柬咸威歪研砖厘厚砌砂泵砚砍面耐耍牵鸥残殃轴轻鸦皆韭背战点虐临览竖省削尝昧盹是盼眨哇哄哑显冒映星昨咧昭畏趴胃贵界虹虾蚁思蚂虽品咽骂勋哗咱响哈哆咬咳咪哪哟炭峡罚贱贴贻骨幽钙钝钞钟钢钠钥钦钧钩钮卸缸拜看矩毡氢怎牲选适秒香种秋科重复竿段便俩贷顺修俏保促俄俐侮俭俗俘信皇泉鬼侵禹侯追俊盾待徊衍律很须叙剑逃食盆胚胧胆胜胞胖脉胎勉狭狮独狰狡狱狠贸怨急饵饶蚀饺饼峦弯将奖哀亭亮度迹庭疮疯疫疤咨姿亲音帝施闺闻闽阀阁差养美姜叛送类迷籽娄前首逆兹总炼炸烁炮炫烂剃洼洁洪洒柒浇浊洞测洗活派洽染洛浏济洋洲浑浓津恃恒恢恍恬恤恰恼恨举觉宣宦室宫宪突穿窃客诫冠诬语扁袄祖神祝祠误诱诲说诵垦退既屋昼屏屎费陡逊眉孩陨除险院娃姥姨姻娇姚娜怒架贺盈勇怠癸蚤柔垒绑绒结绕骄绘给绚骆络绝绞骇统耕耘耗耙艳泰秦珠班素匿蚕顽盏匪捞栽捕埂捂振载赶起盐捎捍捏埋捉捆捐损袁捌都哲逝捡挫换挽挚热恐捣壶捅埃挨耻耿耽聂恭莽莱莲莫莉荷获晋恶莹莺真框梆桂桔栖档桐株桥桦栓桃格桩校核样根索哥速逗栗贾酌配翅辱唇夏砸砰砾础破原套逐烈殊殉顾轿较顿毙致柴桌虑监紧党逞晒眠晓哮唠鸭晃哺晌剔晕蚌畔蚣蚊蚪蚓哨哩圃哭哦恩鸯唤唁哼唧啊唉唆罢峭峨峰圆峻贼贿赂赃钱钳钻钾铁铃铅缺氧氨特牺造乘敌秤租积秧秩称秘透笔笑笋债借值倚俺倾倒倘俱倡候赁俯倍倦健臭射躬息倔徒徐殷舰舱般航途拿耸爹舀爱豺豹颁颂翁胰脆脂胸胳脏脐胶脑脓逛狸狼卿逢鸵留鸳皱饿馁凌凄恋桨浆衰衷高郭席准座症病疾斋疹疼疲脊效离紊唐瓷资凉站剖竞部旁旅畜阅羞羔瓶拳粉料益兼烤烘烦烧烛烟烙递涛浙涝浦酒涉消涡浩海涂浴浮涣涤流润涧涕浪浸涨烫涩涌悖悟悄悍悔悯悦害宽家宵宴宾窍窄容宰案请朗诸诺读扇诽袜袖袍被祥课冥谁调冤谅谆谈谊剥恳展剧屑弱陵祟陶陷陪娱娟恕娥娘通能难预桑绢绣验继骏球琐理琉琅捧堵措描域捺掩捷排焉掉捶赦堆推埠掀授捻教掏掐掠掂培接掷控探据掘掺职基聆勘聊娶著菱勒黄菲萌萝菌萎菜萄菊菩萍菠萤营乾萧萨菇械彬梦婪梗梧梢梅检梳梯桶梭救曹副票酝酗厢戚硅硕奢盔爽聋袭盛匾雪辅辆颅虚彪雀堂常眶匙晨睁眯眼悬野啪啦曼晦晚啄啡距趾啃跃略蚯蛀蛇唬累鄂唱患啰唾唯啤啥啸崖崎崭逻崔帷崩崇崛婴圈铐铛铝铜铭铲银矫甜秸梨犁秽移笨笼笛笙符第敏做袋悠偿偶偎偷您售停偏躯兜假衅徘徙得衔盘舶船舵斜盒鸽敛悉欲彩领脚脖脯豚脸脱象够逸猜猪猎猫凰猖猛祭馅馆凑减毫烹庶麻庵痊痒痕廊康庸鹿盗章竟商族旋望率阎阐着羚盖眷粘粗粒断剪兽焊焕清添鸿淋涯淹渠渐淑淌混淮淆渊淫渔淘淳液淤淡淀深涮涵婆梁渗情惜惭悼惧惕惟惊惦悴惋惨惯寇寅寄寂宿窒窑密谋谍谎谐袱祷祸谓谚谜逮敢尉屠弹隋堕随蛋隅隆隐婚婶婉颇颈绩绪续骑绰绳维绵绷绸综绽绿缀巢琴琳琢琼斑替揍款堪塔搭堰揩越趁趋超揽堤提博揭喜彭揣插揪搜煮援搀裁搁搓搂搅壹握搔揉斯期欺联葫散惹葬募葛董葡敬葱蒋蒂落韩朝辜葵棒棱棋椰植森焚椅椒棵棍椎棉棚棕棺榔椭惠惑逼粟棘酣酥厨厦硬硝确硫雁殖裂雄颊雳暂雅翘辈悲紫凿辉敞棠赏掌晴睐暑最晰量鼎喷喳晶喇遇喊遏晾景畴践跋跌跑跛遗蛙蛛蜓蜒蛤喝鹃喂喘喉喻啼喧嵌幅帽赋赌赎赐赔黑铸铺链销锁锄锅锈锋锌锐甥掰短智氮毯氯鹅剩稍程稀税筐等筑策筛筒筏答筋筝傲傅牌堡集焦傍储皓皖粤奥街惩御循艇舒逾番释禽腊脾腋腔腕鲁猩猬猾猴惫然馈馋装蛮就敦斌痘痢痪痛童竣阔善翔羡普粪尊奠道遂曾焰港滞湖湘渣渤渺湿温渴溃溅滑湃渝湾渡游滋渲溉愤慌惰愕愣惶愧愉慨割寒富寓窜窝窖窗窘遍雇裕裤裙禅禄谢谣谤谦犀属屡强粥疏隔隙隘媒絮嫂媚婿登缅缆缉缎缓缔缕骗编骚缘瑟鹉瑞瑰瑙魂肆摄摸填搏塌鼓摆携搬摇搞塘摊聘斟蒜勤靴靶鹊蓝墓幕蓬蓄蒲蓉蒙蒸献椿禁楚楷榄想槐榆楼概赖酪酬感碍碘碑碎碰碗碌尴雷零雾雹辐辑输督频龄鉴睛睹睦瞄睫睡睬嗜鄙嗦愚暖盟歇暗暇照畸跨跷跳跺跪路跤跟遣蜈蜗蛾蜂蜕嗅嗡嗓署置罪罩蜀幌错锚锡锣锤锥锦键锯锰矮辞稚稠颓愁筹签简筷毁舅鼠催傻像躲魁衙微愈遥腻腰腥腮腹腺鹏腾腿鲍猿颖触解煞雏馍馏酱禀痹廓痴痰廉靖新韵意誊粮数煎塑慈煤煌满漠滇源滤滥滔溪溜漓滚溢溯滨溶溺粱滩慎誉塞寞窥窟寝谨褂裸福谬群殿辟障媳嫉嫌嫁叠缚缝缠缤剿静碧璃赘熬墙墟嘉摧赫截誓境摘摔撇聚慕暮摹蔓蔑蔡蔗蔽蔼熙蔚兢模槛榴榜榨榕歌遭酵酷酿酸碟碱碳磁愿需辖辗雌裳颗瞅墅嗽踊蜻蜡蝇蜘蝉嘛嘀赚锹锻镀舞舔稳熏箕算箩管箫舆僚僧鼻魄魅貌膜膊膀鲜疑孵馒裹敲豪膏遮腐瘩瘟瘦辣彰竭端旗精粹歉弊熄熔煽潇漆漱漂漫滴漾演漏慢慷寨赛寡察蜜寥谭肇褐褪谱隧嫩翠熊凳骡缩慧撵撕撒撩趣趟撑撮撬播擒墩撞撤增撰聪鞋鞍蕉蕊蔬蕴横槽樱橡樟橄敷豌飘醋醇醉磕磊磅碾震霄霉瞒题暴瞎嘻嘶嘲嘹影踢踏踩踪蝶蝴蝠蝎蝌蝗蝙嘿嘱幢墨镇镐镑靠稽稻黎稿稼箱篓箭篇僵躺僻德艘膝膛鲤鲫熟摩褒瘪瘤瘫凛颜毅糊遵憋潜澎潮潭鲨澳潘澈澜澄懂憔懊憎额翩褥谴鹤憨慰劈履豫缭撼擂操擅燕蕾薯薛薇擎薪薄颠翰噩橱橙橘整融瓢醒霍霎辙冀餐嘴踱蹄蹂蟆螃器噪鹦赠默黔镜赞穆篮篡篷篱儒邀衡膨雕鲸磨瘾瘸凝辨辩糙糖糕燃濒澡激懒憾懈窿壁避缰缴戴擦藉鞠藏藐檬檐檀礁磷霜霞瞭瞧瞬瞳瞩瞪曙蹋蹈螺蟋蟀嚎赡穗魏簧簇繁徽爵朦臊鳄癌辫赢糟糠燥懦豁臀臂翼骤藕鞭藤覆瞻蹦嚣镰翻鳍鹰瀑襟璧戳孽警蘑藻攀曝蹲蹭蹬巅簸簿蟹颤靡癣瓣羹鳖爆疆鬓壤馨耀躁蠕嚼嚷巍籍鳞魔糯灌譬蠢霸露霹躏黯髓赣囊镶瓤罐矗", 18);
            MouseHandled = false;
        }

        public static void MovetoTop(Window window)
        {
            Childrens.Insert(0, window);
            FocusWindow = window;
        }

        public static void DrawAll()
        {
            for (int i = 0;  i < Childrens.Count; i++)
            {
                if (Childrens[i].Visible)
                    Childrens[i].OnDraw();
            }
        }

        public static void UpdateAll()
        {
            for (int i = 0 ; i < Childrens.Count; i++)
            {
                if (Childrens[i].Visible)
                    Childrens[i].OnUpdate();
            }
        }

        public static void InputAll()
        {
            for (int i = 0; i < Childrens.Count; i++)
            {
                if (Childrens[i].Visible)
                    Childrens[i].OnInput();
            }
        }

        public static bool HasWindowMoving = false;
        public static Window FocusWindow;
        public static Widget FocusControl;
        public static bool MouseHandled
        {
            get => HasWindowMoving;
            set => HasWindowMoving = value;
        }

    }
}
#endif