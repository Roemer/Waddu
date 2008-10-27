using System.IO;
using System.Xml;
using Waddu.BusinessObjects;
using Waddu.Types;

namespace Waddu.Classes
{
    public class Mapper
    {
        private XmlDocument _xmlDoc;
        private string _xmlFile;

        private Mapper(string mappingFile)
        {
            _xmlFile = mappingFile;

            File.Delete(_xmlFile);
            if (!File.Exists(_xmlFile))
            {
                XmlTextWriter w = new XmlTextWriter(_xmlFile, null);
                w.WriteStartDocument();
                w.WriteStartElement("Waddu_Mappings");
                w.WriteEndElement();
                w.WriteEndDocument();
                w.Close();
            }

            _xmlDoc = new XmlDocument();
            _xmlDoc.Load(_xmlFile);

            FillInitialMappings();
        }

        public static void CreateMapping(string mappingFile)
        {
            Mapper map = new Mapper(mappingFile);
        }

        private XmlElement FindGameElement(string gameName, bool createIfNeeded)
        {
            // Find Game Element
            XmlElement gameElement = _xmlDoc.DocumentElement.SelectSingleNode(string.Format(@"Game[@Name=""{0}""]", gameName)) as XmlElement;

            if (gameElement == null && createIfNeeded)
            {
                // Create if it doesn't exist yet
                gameElement = _xmlDoc.CreateElement("Game");
                XmlAttribute gameAtt = _xmlDoc.CreateAttribute("Name");
                gameAtt.Value = gameName;
                gameElement.Attributes.Append(gameAtt);
                // Append Addon to Document
                _xmlDoc.DocumentElement.AppendChild(gameElement);
            }
            return gameElement;
        }

        private XmlElement FindAddonElement(string addonName, bool createIfNeeded)
        {
            // Find Game Element
            XmlElement gameElement = FindGameElement("World of Warcraft", true);
            if (gameElement == null)
            {
                return null;
            }

            // Search for an Existing Addon
            XmlElement addonElement = gameElement.SelectSingleNode(string.Format(@"Addon[@Name=""{0}""]", addonName)) as XmlElement;
            if (addonElement == null && createIfNeeded)
            {
                // Create if it doesn't exist yet
                addonElement = _xmlDoc.CreateElement("Addon");
                XmlAttribute addonAtt = _xmlDoc.CreateAttribute("Name");
                addonAtt.Value = addonName;
                addonElement.Attributes.Append(addonAtt);
                // Append Addon to Document
                gameElement.AppendChild(addonElement);
            }
            return addonElement;
        }

        private XmlElement FindElement(XmlElement parentElement, string elementName, bool createIfNeeded)
        {
            XmlElement subElement = parentElement.SelectSingleNode(elementName) as XmlElement;
            if (subElement == null && createIfNeeded)
            {
                // Create if it doesn't exist yet
                subElement = _xmlDoc.CreateElement(elementName);
                // Append the Sub Element
                parentElement.AppendChild(subElement);
            }
            return subElement;
        }

        private void AddSubAddon(string addonName, string subAddonName)
        {
            XmlElement addonElement = FindAddonElement(addonName, true);
            XmlElement subAddonsElement = FindElement(addonElement, "SubAddons", true);

            // SubAddon Element
            XmlElement subAddonElement = _xmlDoc.CreateElement("SubAddon");
            // Name Attribute
            XmlAttribute nameAttribute = _xmlDoc.CreateAttribute("Name");
            nameAttribute.Value = subAddonName;
            subAddonElement.Attributes.Append(nameAttribute);
            // Append SubAddon to SubAddons
            subAddonsElement.AppendChild(subAddonElement);
        }

        private void AddMapping(string addonName, string addonTag, AddonSiteId addonSiteId)
        {
            XmlElement addonElement = FindAddonElement(addonName, true);
            XmlElement mappingsElement = FindElement(addonElement, "Mappings", true);

            // Mapping Element
            XmlElement mappingElement = _xmlDoc.CreateElement("Mapping");
            // Site Attribute
            XmlAttribute siteAttribute = _xmlDoc.CreateAttribute("Site");
            siteAttribute.Value = addonSiteId.ToString();
            mappingElement.Attributes.Append(siteAttribute);
            // Tag Attribute
            XmlAttribute tagAttribute = _xmlDoc.CreateAttribute("Tag");
            tagAttribute.Value = addonTag;
            mappingElement.Attributes.Append(tagAttribute);
            // Append Mapping to Mappings
            mappingsElement.AppendChild(mappingElement);
        }

        private void FillInitialMappings()
        {
            // WoWAce
            AddMapping("!BugGrabber", "bug-grabber", AddonSiteId.wowace);
            AddMapping("!SurfaceControl", "surface-control", AddonSiteId.wowace);
            AddMapping("Ace", "ace", AddonSiteId.wowace);
            AddMapping("Ace2", "ace2", AddonSiteId.wowace);
            AddMapping("Ace3", "ace3", AddonSiteId.wowace);
            AddMapping("ACP", "acp", AddonSiteId.wowace);
            AddMapping("ag_UnitFrames", "ag_unitframes", AddonSiteId.wowace);
            AddMapping("AHsearch", "ahsearch", AddonSiteId.wowace);
            AddMapping("Aloft", "aloft", AddonSiteId.wowace);
            AddMapping("ampere", "ampere", AddonSiteId.wowace);
            AddMapping("Antagonist", "antagonist", AddonSiteId.wowace);
            AddMapping("ArenaPointer", "arena-pointer", AddonSiteId.wowace);
            AddMapping("ArkInventory", "ark-inventory", AddonSiteId.wowace);
            AddMapping("AtlasLoot", "atlasloot-enhanced", AddonSiteId.wowace);
            AddMapping("aUF_Banzai", "a-uf_banzai", AddonSiteId.wowace);
            AddMapping("AutoBar", "auto-bar", AddonSiteId.wowace);
            AddMapping("BadBoy", "bad-boy", AddonSiteId.wowace);
            AddMapping("Baggins", "baggins", AddonSiteId.wowace);
            AddMapping("Baggins_ClosetGnome", "baggins_closet-gnome", AddonSiteId.wowace);
            AddMapping("Baggins_Outfitter", "baggins_outfitter", AddonSiteId.wowace);
            AddMapping("Baggins_Professions", "baggins_professions", AddonSiteId.wowace);
            AddMapping("Baggins_Search", "baggins_search", AddonSiteId.wowace);
            AddMapping("Baggins_SectionColor", "baggins_section-color", AddonSiteId.wowace);
            AddMapping("Baggins_Usable", "baggins_usable", AddonSiteId.wowace);
            AddMapping("BankItems", "bank-items", AddonSiteId.wowace);
            AddMapping("BankStack", "bank-stack", AddonSiteId.wowace);
            AddMapping("Bartender4", "bartender4", AddonSiteId.wowace);
            AddMapping("BasicBuffs", "basic-buffs", AddonSiteId.wowace);
            AddMapping("beql", "beql", AddonSiteId.wowace);
            AddMapping("BetterInbox", "better-inbox", AddonSiteId.wowace);
            AddMapping("BigBrother", "big-brother", AddonSiteId.wowace);
            AddMapping("BigWigs", "big-wigs", AddonSiteId.wowace);
            AddMapping("BigWigs_KalecgosHealth", "bwkh", AddonSiteId.wowace);
            AddMapping("Broker_Clock", "broker_clock", AddonSiteId.wowace);
            AddMapping("Broker_Mail", "broker_mail", AddonSiteId.wowace);
            AddMapping("Broker2FuBar", "broker2fubar", AddonSiteId.wowace);
            AddMapping("Buffalo", "buffalo", AddonSiteId.wowace);
            AddMapping("BuffEnough", "buff-enough", AddonSiteId.wowace);
            AddMapping("BugSack", "bugsack", AddonSiteId.wowace);
            AddMapping("ButtonFacade", "buttonfacade", AddonSiteId.wowace);
            AddMapping("ButtonFacade_Apathy", "buttonfacade_apathy", AddonSiteId.wowace);
            AddMapping("ButtonFacade_Caith", "buttonfacade_caith", AddonSiteId.wowace);
            AddMapping("ButtonFacade_Entropy", "buttonfacade_entropy", AddonSiteId.wowace);
            AddMapping("ButtonFacade_Gears", "buttonfacade_gears", AddonSiteId.wowace);
            AddMapping("ButtonFacade_HKitty", "button-facade_hkitty", AddonSiteId.wowace);
            AddMapping("ButtonFacade_LiteStep", "buttonfacade_litestep", AddonSiteId.wowace);
            AddMapping("ButtonFacade_Onyx", "buttonfacade_onyx", AddonSiteId.wowace);
            AddMapping("ButtonFacade_Serenity", "buttonfacade_serenity", AddonSiteId.wowace);
            AddMapping("ButtonFacade_simpleSquare", "button-facade_simple-square", AddonSiteId.wowace);
            AddMapping("Capping", "capping", AddonSiteId.wowace);
            AddMapping("Cartographer", "cartographer", AddonSiteId.wowace);
            AddMapping("CCBreaker", "ccbreaker", AddonSiteId.wowace);
            AddMapping("Chaman2", "chaman2", AddonSiteId.wowace);
            AddMapping("Chatter", "chatter", AddonSiteId.wowace);
            AddMapping("Chinchilla", "chinchilla", AddonSiteId.wowace);
            AddMapping("ClassTimer", "classtimer", AddonSiteId.wowace);
            AddMapping("ClearFont2", "clear-font2", AddonSiteId.wowace);
            AddMapping("ClearFont2_FontPack", "clear-font2_font-pack", AddonSiteId.wowace);
            AddMapping("Click2Cast", "click2cast", AddonSiteId.wowace);
            AddMapping("ClosetGnome", "closet-gnome", AddonSiteId.wowace);
            AddMapping("ClosetGnome_Banker", "closet-gnome_banker", AddonSiteId.wowace);
            AddMapping("ClosetGnome_HelmNCloak", "closet-gnome_helm-ncloak", AddonSiteId.wowace);
            AddMapping("ClosetGnome_Mount", "closet-gnome_mount", AddonSiteId.wowace);
            AddMapping("ClosetGnome_Tooltip", "closet-gnome_tooltip", AddonSiteId.wowace);
            AddMapping("Coconuts", "coconuts", AddonSiteId.wowace);
            AddMapping("Combuctor", "combuctor", AddonSiteId.wowace);
            AddMapping("CooldownCount", "cooldown-count", AddonSiteId.wowace);
            AddMapping("CooldownTimers2", "cooldown-timers2", AddonSiteId.wowace);
            AddMapping("CowTip", "cowtip", AddonSiteId.wowace);
            AddMapping("Cryolysis2", "cryolysis2", AddonSiteId.wowace);
            AddMapping("Damnation", "damnation", AddonSiteId.wowace);
            AddMapping("DBM_API", "deadly-boss-mods", AddonSiteId.wowace);
            AddMapping("Dock", "dock", AddonSiteId.wowace);
            AddMapping("DotDotDot", "dot-dot-dot", AddonSiteId.wowace);
            AddMapping("DrDamage", "dr-damage", AddonSiteId.wowace);
            AddMapping("EavesDrop", "eaves-drop", AddonSiteId.wowace);
            AddMapping("eePanels2", "ee-panels2", AddonSiteId.wowace);
            AddMapping("ElkBuffBars", "elkbuffbars", AddonSiteId.wowace);
            AddMapping("EQCompare", "eqcompare", AddonSiteId.wowace);
            AddMapping("ErrorMonster", "error-monster", AddonSiteId.wowace);
            AddMapping("EveryQuest", "everyquest", AddonSiteId.wowace);
            AddMapping("Fence", "fence", AddonSiteId.wowace);
            AddMapping("FishermansFriend", "fishermans-friend", AddonSiteId.wowace);
            AddMapping("Fortress", "fortress", AddonSiteId.wowace);
            AddMapping("FreeRefills", "free-refills", AddonSiteId.wowace);
            AddMapping("FuBar", "fubar", AddonSiteId.wowace);
            AddMapping("FuBar_AmmoFu", "fubar_ammofu", AddonSiteId.wowace);
            AddMapping("FuBar_BagFu", "fu-bar_bag-fu", AddonSiteId.wowace);
            AddMapping("FuBar_ClockFu", "fubar_clockfu", AddonSiteId.wowace);
            AddMapping("FuBar_DurabilityFu", "fubar_durabilityfu", AddonSiteId.wowace);
            AddMapping("FuBar_DuraTek", "fu-bar_dura-tek", AddonSiteId.wowace);
            AddMapping("FuBar_ExperienceFu", "fubar_experiencefu", AddonSiteId.wowace);
            AddMapping("FuBar_FactionsFu", "fubar_factionsfu", AddonSiteId.wowace);
            AddMapping("FuBar_FriendsFu", "fubar_friendsfu", AddonSiteId.wowace);
            AddMapping("FuBar_FuXPFu", "fu-bar_fu-xpfu", AddonSiteId.wowace);
            AddMapping("FuBar_GarbageFu", "fu-bar_garbage-fu", AddonSiteId.wowace);
            AddMapping("FuBar_GroupFu", "fubar_groupfu", AddonSiteId.wowace);
            AddMapping("FuBar_GuildFu", "fubar_guildfu", AddonSiteId.wowace);
            AddMapping("FuBar_HonorFu", "fubar_honorfu", AddonSiteId.wowace);
            AddMapping("FuBar_ItemBonusesFu", "fu-bar_item-bonuses-fu", AddonSiteId.wowace);
            AddMapping("FuBar_LocationFu", "fubar_locationfu", AddonSiteId.wowace);
            AddMapping("FuBar_MicroMenuFu", "fubar_micromenufu", AddonSiteId.wowace);
            AddMapping("FuBar_MoneyFu", "fu-bar_money-fu", AddonSiteId.wowace);
            AddMapping("FuBar_NameToggleFu", "fubar_nametogglefu", AddonSiteId.wowace);
            AddMapping("FuBar_PerformanceFu", "fubar_performancefu", AddonSiteId.wowace);
            AddMapping("FuBar_PetFu", "fu-bar_pet-fu", AddonSiteId.wowace);
            AddMapping("FuBar_QuestsFu", "fu-bar_quests-fu", AddonSiteId.wowace);
            AddMapping("FuBar_RaidBuffFu", "fu-bar_raid-buff-fu", AddonSiteId.wowace);
            AddMapping("FuBar_RecountFu", "fu-bar_recount-fu", AddonSiteId.wowace);
            AddMapping("FuBar_ReagentFu", "fu-bar_reagent-fu", AddonSiteId.wowace);
            AddMapping("FuBar_SkillsPlusFu", "fu-bar_skills-plus-fu", AddonSiteId.wowace);
            AddMapping("FuBar_SpeedFu", "fu-bar_speed-fu", AddonSiteId.wowace);
            AddMapping("FuBar_ToFu", "fu-bar_to-fu", AddonSiteId.wowace);
            AddMapping("FuBar_TopScoreFu", "fu-bar_top-score-fu", AddonSiteId.wowace);
            AddMapping("FuBar_TrainerFu", "fu-bar_trainer-fu", AddonSiteId.wowace);
            AddMapping("FuBar_TransporterFu", "fu-bar_transporter-fu", AddonSiteId.wowace);
            AddMapping("FuBar_VolumeFu", "fu-bar_volume-fu", AddonSiteId.wowace);
            AddMapping("FuBar2Broker", "fubar2broker", AddonSiteId.wowace);
            AddMapping("FuTextures", "futextures", AddonSiteId.wowace);
            AddMapping("GatherMate", "gathermate", AddonSiteId.wowace);
            AddMapping("GatherMate_Data", "gathermate_data", AddonSiteId.wowace);
            AddMapping("GatherMate_Sharing", "gathermate_sharing", AddonSiteId.wowace);
            AddMapping("GloryLib", "glorylib", AddonSiteId.wowace);
            AddMapping("Grid", "grid", AddonSiteId.wowace);
            AddMapping("GridIndicatorSideIcons", "grid-indicator-side-icons", AddonSiteId.wowace);
            AddMapping("GridIndicatorText3", "grid-indicator-text3", AddonSiteId.wowace);
            AddMapping("GridManaBars", "grid-mana-bars", AddonSiteId.wowace);
            AddMapping("GridSideIndicators", "grid-side-indicators", AddonSiteId.wowace);
            AddMapping("GridStatusAFK", "grid-status-afk", AddonSiteId.wowace);
            AddMapping("GridStatusHots", "grid-status-hots", AddonSiteId.wowace);
            AddMapping("GridStatusMissingBuffs", "grid-status-missing-buffs", AddonSiteId.wowace);
            AddMapping("GridStatusRaidIcons", "grid-status-raid-icons", AddonSiteId.wowace);
            AddMapping("GridStatusReadyCheck", "grid-status-ready-check", AddonSiteId.wowace);
            AddMapping("Grid2", "grid2", AddonSiteId.wowace);
            AddMapping("GrimReaper", "grim-reaper", AddonSiteId.wowace);
            AddMapping("HandyNotes", "handynotes", AddonSiteId.wowace);
            AddMapping("HandyNotes_FlightMasters", "handy-notes_flight-masters", AddonSiteId.wowace);
            AddMapping("HandyNotes_Guild", "handynotes_guild", AddonSiteId.wowace);
            AddMapping("HandyNotes_Mailboxes", "handy-notes_mailboxes", AddonSiteId.wowace);
            AddMapping("HeadCount", "head-count", AddonSiteId.wowace);
            AddMapping("InstanceMaps", "instance-maps", AddonSiteId.wowace);
            AddMapping("ItemPriceTooltip", "item-price-tooltip", AddonSiteId.wowace);
            AddMapping("kgPanels", "kg-panels", AddonSiteId.wowace);
            AddMapping("LD50_abar", "ld50_abar", AddonSiteId.wowace);
            AddMapping("Links", "links", AddonSiteId.wowace);
            AddMapping("LittleWigs", "little-wigs", AddonSiteId.wowace);
            AddMapping("MagicDKP", "magic-dk", AddonSiteId.wowace);
            AddMapping("MainAssist", "main-assist", AddonSiteId.wowace);
            AddMapping("Manufac", "manufac", AddonSiteId.wowace);
            AddMapping("Mapster", "mapster", AddonSiteId.wowace);
            AddMapping("MBB", "mbb", AddonSiteId.wowace);
            AddMapping("Mendeleev", "mendeleev", AddonSiteId.wowace);
            AddMapping("Minimalist", "minimalist", AddonSiteId.wowace);
            AddMapping("MiniMapster", "minimapster", AddonSiteId.wowace);
            AddMapping("MobSpells", "mob-spells", AddonSiteId.wowace);
            AddMapping("MobHealth", "mob-health", AddonSiteId.wowace);
            AddMapping("MobHealth3_BlizzardFrames", "mob-health3_blizzard-frames", AddonSiteId.wowace);
            AddMapping("Niagara", "niagara", AddonSiteId.wowace);
            AddMapping("Numen", "numen", AddonSiteId.wowace);
            AddMapping("nQuestLog", "n-quest-log", AddonSiteId.wowace);
            AddMapping("Omen", "omen-threat-meter", AddonSiteId.wowace);
            AddMapping("OneBag3", "onebag3", AddonSiteId.wowace);
            AddMapping("OptiTaunt", "opti-taunt", AddonSiteId.wowace);
            AddMapping("oRA2", "ora2", AddonSiteId.wowace);
            AddMapping("Parrot", "parrot", AddonSiteId.wowace);
            AddMapping("PitBull", "pit-bull", AddonSiteId.wowace);
            AddMapping("PitBull_QuickHealth", "pit-bull_quick-health", AddonSiteId.wowace);
            AddMapping("Postal", "postal", AddonSiteId.wowace);
            AddMapping("Prat-3.0", "prat-3-0", AddonSiteId.wowace);
            AddMapping("ProfessionsBook", "professions-book", AddonSiteId.wowace);
            AddMapping("Proximo", "proximo", AddonSiteId.wowace);
            AddMapping("Quartz", "quartz", AddonSiteId.wowace);
            AddMapping("Quixote", "quixote", AddonSiteId.wowace);
            AddMapping("RangeRecolor", "range-recolor", AddonSiteId.wowace);
            AddMapping("RatingBuster", "rating-buster", AddonSiteId.wowace);
            AddMapping("Recount", "recount", AddonSiteId.wowace);
            AddMapping("Rep2", "rep2", AddonSiteId.wowace);
            AddMapping("Reslack", "reslack", AddonSiteId.wowace);
            AddMapping("RightWing", "right-wing", AddonSiteId.wowace);
            AddMapping("Routes", "routes", AddonSiteId.wowace);
            AddMapping("rSelfCast", "r-self-cast", AddonSiteId.wowace);
            AddMapping("SalvationKiller", "salvation-killer", AddonSiteId.wowace);
            AddMapping("sct", "sct", AddonSiteId.wowace);
            AddMapping("sct_options", "sct_options", AddonSiteId.wowace);
            AddMapping("SharedMedia", "sharedmedia", AddonSiteId.wowace);
            AddMapping("SharedMediaAdditionalFonts", "shared-media-additional-fonts", AddonSiteId.wowace);
            AddMapping("simpleMinimap", "simple-minimap", AddonSiteId.wowace);
            AddMapping("Skillet", "skillet", AddonSiteId.wowace);
            AddMapping("Skinner", "skinner", AddonSiteId.wowace);
            AddMapping("SnapshotBar", "snapshot-bar", AddonSiteId.wowace);
            AddMapping("Sphere", "sphere", AddonSiteId.wowace);
            AddMapping("sRaidFrames", "sraidframes", AddonSiteId.wowace);
            AddMapping("StatBlockCore", "stat-block-core", AddonSiteId.wowace);
            AddMapping("StatBlock_Durability", "stat-block_durability", AddonSiteId.wowace);
            AddMapping("StatBlock_FPS", "stat-block_fps", AddonSiteId.wowace);
            AddMapping("StatBlock_Latency", "stat-block_latency", AddonSiteId.wowace);
            AddMapping("StatBlock_Memory", "stat-block_memory", AddonSiteId.wowace);
            AddMapping("Talented", "talented", AddonSiteId.wowace);
            AddMapping("Talented_Data", "talented_data", AddonSiteId.wowace);
            AddMapping("Talented_Loader", "talented_loader", AddonSiteId.wowace);
            AddMapping("TouristLib", "touristlib", AddonSiteId.wowace);
            AddMapping("Transcriptor", "transcriptor", AddonSiteId.wowace);
            AddMapping("WitchHunt", "witch-hunt", AddonSiteId.wowace);
            AddMapping("WoWEquip", "wowequip", AddonSiteId.wowace);
            AddMapping("XLoot", "xloot", AddonSiteId.wowace);
            AddMapping("XLootGroup", "xloot-group", AddonSiteId.wowace);
            AddMapping("XLootMaster", "xloot-master", AddonSiteId.wowace);
            AddMapping("XLootMonitor", "xloot-monitor", AddonSiteId.wowace);
            AddMapping("XPerl", "xperl", AddonSiteId.wowace);
            AddMapping("XRS", "xrs", AddonSiteId.wowace);
            AddMapping("Yata", "yata", AddonSiteId.wowace);
            AddMapping("Yatba", "yatba", AddonSiteId.wowace);
            AddMapping("YurrCombatLog", "yurr-combat-log", AddonSiteId.wowace);
            AddMapping("ZOMGBuffs", "zomgbuffs", AddonSiteId.wowace);

            // Blizzard
            AddMapping("Blizzard_AchievementUI", "Blizzard_AchievementUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_AuctionUI", "Blizzard_AuctionUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_BarbershopUI", "Blizzard_BarbershopUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_BattlefieldMinimap", "Blizzard_BattlefieldMinimap", AddonSiteId.blizzard);
            AddMapping("Blizzard_BindingUI", "Blizzard_BindingUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_Calendar", "Blizzard_Calendar", AddonSiteId.blizzard);
            AddMapping("Blizzard_CombatLog", "Blizzard_CombatLog", AddonSiteId.blizzard);
            AddMapping("Blizzard_CombatText", "Blizzard_CombatText", AddonSiteId.blizzard);
            AddMapping("Blizzard_CraftUI", "Blizzard_CraftUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_FeedbackUI", "Blizzard_FeedbackUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_GlyphUI", "Blizzard_GlyphUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_GMSurveyUI", "Blizzard_GMSurveyUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_GuildBankUI", "Blizzard_GuildBankUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_InspectUI", "Blizzard_InspectUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_ItemSocketingUI", "Blizzard_ItemSocketingUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_MacroUI", "Blizzard_MacroUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_RaidUI", "Blizzard_RaidUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_TalentUI", "Blizzard_TalentUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_TimeManager", "Blizzard_TimeManager", AddonSiteId.blizzard);
            AddMapping("Blizzard_TokenUI", "Blizzard_TokenUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_TradeSkillUI", "Blizzard_TradeSkillUI", AddonSiteId.blizzard);
            AddMapping("Blizzard_TrainerUI", "Blizzard_TrainerUI", AddonSiteId.blizzard);

            // WoWInterface
            AddMapping("!!Warmup", "4939", AddonSiteId.wowinterface); // RAR
            AddMapping("!StopTheSpam", "4077", AddonSiteId.wowinterface);
            AddMapping("AckisRecipeList", "8512", AddonSiteId.wowinterface);
            AddMapping("AddonManager", "7164", AddonSiteId.wowinterface);
            AddMapping("Afflicted2", "8063", AddonSiteId.wowinterface);
            AddMapping("Aloft", "10864", AddonSiteId.wowinterface);
            AddMapping("Atlas", "3896", AddonSiteId.wowinterface);
            AddMapping("Bongos", "8419", AddonSiteId.wowinterface);
            AddMapping("ButtonFacade_Elegance", "9623", AddonSiteId.wowinterface);
            AddMapping("ButtonFacade_Sleek", "11010", AddonSiteId.wowinterface);
            AddMapping("ButtonFacade_Tones", "9712", AddonSiteId.wowinterface);
            AddMapping("Capping", "11177", AddonSiteId.wowinterface);
            AddMapping("Clique", "5108", AddonSiteId.wowinterface);
            AddMapping("DPS", "11059", AddonSiteId.wowinterface);
            AddMapping("DRTracker", "8901", AddonSiteId.wowinterface);
            AddMapping("EquipCompare", "4392", AddonSiteId.wowinterface);
            AddMapping("FuBar_AuditorFu", "5429", AddonSiteId.wowinterface);
            AddMapping("GFW_FeedOMatic", "4160", AddonSiteId.wowinterface);
            AddMapping("GhostPulse", "8418", AddonSiteId.wowinterface);
            AddMapping("Hack", "11101", AddonSiteId.wowinterface);
            AddMapping("InFlight", "11178", AddonSiteId.wowinterface);
            AddMapping("ItemRack", "6818", AddonSiteId.wowinterface);
            AddMapping("LitheTooltipDoctor", "5336", AddonSiteId.wowinterface);
            AddMapping("Ludwig", "5174", AddonSiteId.wowinterface);
            AddMapping("Macaroon", "10636", AddonSiteId.wowinterface);
            AddMapping("MikScrollingBattleText", "5153", AddonSiteId.wowinterface);
            AddMapping("MTLove", "7024", AddonSiteId.wowinterface);
            AddMapping("OmniCC", "4836", AddonSiteId.wowinterface);
            AddMapping("OPie", "9094", AddonSiteId.wowinterface);
            AddMapping("Possessions", "6443", AddonSiteId.wowinterface);
            AddMapping("QuestHelper", "9896", AddonSiteId.wowinterface);
            AddMapping("ReagentRestocker", "7569", AddonSiteId.wowinterface);
            AddMapping("Tipachu", "8808", AddonSiteId.wowinterface);
            AddMapping("TomTom", "7032", AddonSiteId.wowinterface);
            AddMapping("TotemManager", "10080", AddonSiteId.wowinterface);
            AddMapping("TourGuide", "7674", AddonSiteId.wowinterface);

            // WoWUI
            AddMapping("Atlas", "400", AddonSiteId.wowui);
            AddMapping("QuestGuru", "4855", AddonSiteId.wowui);
            AddMapping("QuestHelper", "6271", AddonSiteId.wowui);
            AddMapping("RogueFocus", "6060", AddonSiteId.wowui);
            AddMapping("Venantes", "4692", AddonSiteId.wowui);

            // Curseforge
            AddMapping("BloodyFont", "bloody-font-2-0", AddonSiteId.curseforge);
            AddMapping("Chaman2", "totem-manager", AddonSiteId.curseforge);
            AddMapping("Decursive", "decursive", AddonSiteId.curseforge);
            AddMapping("QuestHelper", "quest-helper", AddonSiteId.curseforge);
            AddMapping("AuctionMaster", "vendor", AddonSiteId.curseforge);

            // Curse
            AddMapping("Atlas", "atlas", AddonSiteId.curse);
            AddMapping("Bagnon", "bagnon", AddonSiteId.curse);
            AddMapping("Carbonite", "carbonite-quest", AddonSiteId.curse);
            AddMapping("DruidBar", "druid-bar", AddonSiteId.curse);
            AddMapping("DynamicPerformance", "dynamic-performance", AddonSiteId.curse);
            AddMapping("Examiner", "examiner", AddonSiteId.curse);
            AddMapping("FlightMap", "flight-map", AddonSiteId.curse);
            AddMapping("FocusFrame", "focus-frame", AddonSiteId.curse);
            AddMapping("GoGoMount", "go-go-mount", AddonSiteId.curse);
            AddMapping("ItemRack", "item-rack", AddonSiteId.curse);
            AddMapping("MikScrollingBattleText", "mik-scrolling-battle-text", AddonSiteId.curse);
            AddMapping("Necrosis", "necrosis-ld-c", AddonSiteId.curse);
            AddMapping("OmniCC", "omni-cc", AddonSiteId.curse);
            AddMapping("Outfitter", "outfitter", AddonSiteId.curse);
            AddMapping("QuestGuru", "quest-guru", AddonSiteId.curse);
            AddMapping("QuestHelper", "quest-helper", AddonSiteId.curse);
            AddMapping("QuickStatsV2", "quick-stats-v2", AddonSiteId.curse);
            AddMapping("sctd", "sct-damage", AddonSiteId.curse);
            AddMapping("SimpleRaidTargetIcons", "simple-raid-target-icons", AddonSiteId.curse);
            AddMapping("SpamMeNot", "spam-me-not", AddonSiteId.curse);
            AddMapping("SW_Stats", "sw-stats", AddonSiteId.curse);
            AddMapping("TrinketMenu", "trinket-menu", AddonSiteId.curse);
            AddMapping("WhoHas", "who-has", AddonSiteId.curse);
            AddMapping("WIM", "wim-wo-w-instant-messenger", AddonSiteId.curse);
            AddMapping("XLoot", "xloot", AddonSiteId.curse);
            AddMapping("ZHunterMod", "zhunter-mod", AddonSiteId.curse);

            // Special
            AddMapping("CT_Core", "CT_Core", AddonSiteId.wowspecial);
            AddMapping("CT_RaidTracker", "CT_RaidTracker", AddonSiteId.wowspecial);
            AddMapping("CT_UnitFrames", "CT_UnitFrames", AddonSiteId.wowspecial);
            AddMapping("MarsPartyBuff", "MarsPartyBuff", AddonSiteId.wowspecial);
            AddMapping("MobMap", "MobMap", AddonSiteId.wowspecial);
            AddMapping("xchar", "xchar", AddonSiteId.wowspecial);
            AddMapping("xchar_Arygos", "xchar_Arygos", AddonSiteId.wowspecial);
            AddMapping("xchar_Kargath", "xchar_Kargath", AddonSiteId.wowspecial);
            AddMapping("xchar_MalGanis", "xchar_MalGanis", AddonSiteId.wowspecial);

            // Direct
            AddMapping("LunarSphere", "0.803|19.10.2008|http://moongazeaddons.proboards79.com/index.cgi?board=lunarsphere&action=display&thread=728|http://www.lunaraddons.com/LunarSphere_803.zip", AddonSiteId.direct);

            // SubAddons
            // ag_UnitFrames
            AddSubAddon("ag_UnitFrames", "ag_Extras");
            AddSubAddon("ag_UnitFrames", "ag_Options");
            // Atlas
            AddSubAddon("Atlas", "Atlas_Battlegrounds");
            AddSubAddon("Atlas", "Atlas_DungeonLocs");
            AddSubAddon("Atlas", "Atlas_FlightPaths");
            AddSubAddon("Atlas", "Atlas_OutdoorRaids");
            // AtlasLoot
            AddSubAddon("AtlasLoot", "AtlasLoot_BurningCrusade");
            AddSubAddon("AtlasLoot", "AtlasLoot_Crafting");
            AddSubAddon("AtlasLoot", "AtlasLoot_OriginalWoW");
            AddSubAddon("AtlasLoot", "AtlasLoot_WorldEvents");
            AddSubAddon("AtlasLoot", "AtlasLoot_WrathoftheLichKing");
            // Bagnon
            AddSubAddon("Bagnon", "Bagnon_Forever");
            AddSubAddon("Bagnon", "Bagnon_Options");
            AddSubAddon("Bagnon", "Bagnon_Tooltips");
            // BigWigs
            AddSubAddon("BigWigs", "BigWigs_BlackTemple");
            AddSubAddon("BigWigs", "BigWigs_Extras");
            AddSubAddon("BigWigs", "BigWigs_Hyjal");
            AddSubAddon("BigWigs", "BigWigs_Karazhan");
            AddSubAddon("BigWigs", "BigWigs_Naxxramas");
            AddSubAddon("BigWigs", "BigWigs_Outland");
            AddSubAddon("BigWigs", "BigWigs_Plugins");
            AddSubAddon("BigWigs", "BigWigs_SC");
            AddSubAddon("BigWigs", "BigWigs_Sunwell");
            AddSubAddon("BigWigs", "BigWigs_TheEye");
            AddSubAddon("BigWigs", "BigWigs_ZulAman");
            // Bongos
            AddSubAddon("Bongos", "Bongos_AB");
            AddSubAddon("Bongos", "Bongos_CastBar");
            AddSubAddon("Bongos", "Bongos_Options");
            AddSubAddon("Bongos", "Bongos_Roll");
            AddSubAddon("Bongos", "Bongos_Stats");
            AddSubAddon("Bongos", "Bongos_XP");
            // Cartographer
            AddSubAddon("Cartographer", "Cartographer_Battlegrounds");
            AddSubAddon("Cartographer", "Cartographer_Coordinates");
            AddSubAddon("Cartographer", "Cartographer_Foglight");
            AddSubAddon("Cartographer", "Cartographer_GroupColors");
            AddSubAddon("Cartographer", "Cartographer_GuildPositions");
            AddSubAddon("Cartographer", "Cartographer_InstanceLoot");
            AddSubAddon("Cartographer", "Cartographer_InstanceMaps");
            AddSubAddon("Cartographer", "Cartographer_InstanceNotes");
            AddSubAddon("Cartographer", "Cartographer_LookNFeel");
            AddSubAddon("Cartographer", "Cartographer_Notes");
            AddSubAddon("Cartographer", "Cartographer_POI");
            AddSubAddon("Cartographer", "Cartographer_Professions");
            AddSubAddon("Cartographer", "Cartographer_Waypoints");
            AddSubAddon("Cartographer", "Cartographer_ZoneInfo");
            // Combuctor
            AddSubAddon("Combuctor", "Bagnon_Forever");
            AddSubAddon("Combuctor", "Bagnon_Tooltips");
            // Deadly Boss Mods
            AddSubAddon("DBM_API", "DBM_Battlegrounds");
            AddSubAddon("DBM_API", "DBM_BlackTemple");
            AddSubAddon("DBM_API", "DBM_GUI");
            AddSubAddon("DBM_API", "DBM_Hyjal");
            AddSubAddon("DBM_API", "DBM_Karazhan");
            AddSubAddon("DBM_API", "DBM_Outlands");
            AddSubAddon("DBM_API", "DBM_Serpentshrine");
            AddSubAddon("DBM_API", "DBM_Sunwell");
            AddSubAddon("DBM_API", "DBM_TheEye");
            AddSubAddon("DBM_API", "DBM_ZulAman");
            // EveryQuest
            AddSubAddon("EveryQuest", "EveryQuest_Battlegrounds");
            AddSubAddon("EveryQuest", "EveryQuest_Classes");
            AddSubAddon("EveryQuest", "EveryQuest_Dungeons");
            AddSubAddon("EveryQuest", "EveryQuest_Eastern_Kingdoms");
            AddSubAddon("EveryQuest", "EveryQuest_Kalimdor");
            AddSubAddon("EveryQuest", "EveryQuest_Miscellaneous");
            AddSubAddon("EveryQuest", "EveryQuest_Northrend");
            AddSubAddon("EveryQuest", "EveryQuest_Outland");
            AddSubAddon("EveryQuest", "EveryQuest_Professions");
            AddSubAddon("EveryQuest", "EveryQuest_Raids");
            AddSubAddon("EveryQuest", "EveryQuest_Seasonal");
            // Grid2
            AddSubAddon("Grid2", "Grid2Options");
            // InFlight
            AddSubAddon("InFlight", "InFlight_Load");
            // ItemRack
            AddSubAddon("ItemRack", "ItemRackOptions");
            // kgPanels
            AddSubAddon("kgPanels", "kgPanelsConfig");
            // LittleWigs
            AddSubAddon("LittleWigs", "LittleWigs_Auchindoun");
            AddSubAddon("LittleWigs", "LittleWigs_Coilfang");
            AddSubAddon("LittleWigs", "LittleWigs_CoT");
            AddSubAddon("LittleWigs", "LittleWigs_HellfireCitadel");
            AddSubAddon("LittleWigs", "LittleWigs_MagistersTerrace");
            AddSubAddon("LittleWigs", "LittleWigs_TempestKeep");
            // LunarSphere
            AddSubAddon("LunarSphere", "LunarSphereExporter");
            AddSubAddon("LunarSphere", "LunarSphereImports");
            // Macaroon
            AddSubAddon("Macaroon", "MacaroonProfiles");
            // MikScrollingBattleText
            AddSubAddon("MikScrollingBattleText", "MSBTOptions");
            // MobMap
            AddSubAddon("MobMap", "MobMapDatabaseStub1");
            AddSubAddon("MobMap", "MobMapDatabaseStub2");
            AddSubAddon("MobMap", "MobMapDatabaseStub3");
            AddSubAddon("MobMap", "MobMapDatabaseStub4");
            AddSubAddon("MobMap", "MobMapDatabaseStub5");
            AddSubAddon("MobMap", "MobMapDatabaseStub6");
            AddSubAddon("MobMap", "MobMapDatabaseStub7");
            AddSubAddon("MobMap", "MobMapDatabaseStub8");
            AddSubAddon("MobMap", "MobMapDatabaseStub9");
            AddSubAddon("MobMap", "MobMapDatabaseStub10");
            AddSubAddon("MobMap", "MobMapDatabaseStub11");
            AddSubAddon("MobMap", "MobMapDatabaseStub12");
            AddSubAddon("MobMap", "MobMapDatabaseStub13");
            AddSubAddon("MobMap", "MobMapDatabaseStub14");
            // OmniCC
            AddSubAddon("OmniCC", "OmniCC_Options");
            // oRA2
            AddSubAddon("oRA2", "oRA2_Leader");
            AddSubAddon("oRA2", "oRA2_Optional");
            AddSubAddon("oRA2", "oRA2_Participant");
            // PitBull
            AddSubAddon("PitBull", "PitBull_Aura");
            AddSubAddon("PitBull", "PitBull_Banzai");
            AddSubAddon("PitBull", "PitBull_BarFader");
            AddSubAddon("PitBull", "PitBull_CastBar");
            AddSubAddon("PitBull", "PitBull_CombatFader");
            AddSubAddon("PitBull", "PitBull_CombatIcon");
            AddSubAddon("PitBull", "PitBull_CombatText");
            AddSubAddon("PitBull", "PitBull_ComboPoints");
            AddSubAddon("PitBull", "PitBull_DruidManaBar");
            AddSubAddon("PitBull", "PitBull_ExperienceBar");
            AddSubAddon("PitBull", "PitBull_HappinessIcon");
            AddSubAddon("PitBull", "PitBull_HealthBar");
            AddSubAddon("PitBull", "PitBull_HideBlizzard");
            AddSubAddon("PitBull", "PitBull_Highlight");
            AddSubAddon("PitBull", "PitBull_LeaderIcon");
            AddSubAddon("PitBull", "PitBull_MasterLooterIcon");
            AddSubAddon("PitBull", "PitBull_Portrait");
            AddSubAddon("PitBull", "PitBull_PowerBar");
            AddSubAddon("PitBull", "PitBull_PvPIcon");
            AddSubAddon("PitBull", "PitBull_RaidTargetIcon");
            AddSubAddon("PitBull", "PitBull_RangeCheck");
            AddSubAddon("PitBull", "PitBull_ReadyCheckIcon");
            AddSubAddon("PitBull", "PitBull_ReputationBar");
            AddSubAddon("PitBull", "PitBull_RestIcon");
            AddSubAddon("PitBull", "PitBull_Spark");
            AddSubAddon("PitBull", "PitBull_ThreatBar");
            AddSubAddon("PitBull", "PitBull_TotemTimers");
            AddSubAddon("PitBull", "PitBull_VisualHeal");
            AddSubAddon("PitBull", "PitBull_VoiceIcon");
            // Prat
            AddSubAddon("Prat-3.0", "Prat-3.0_HighCPUUsageModules");
            AddSubAddon("Prat-3.0", "Prat-3.0_Libraries");
            // Quartz
            AddSubAddon("Quartz", "Quartz_Buff");
            AddSubAddon("Quartz", "Quartz_Flight");
            AddSubAddon("Quartz", "Quartz_Focus");
            AddSubAddon("Quartz", "Quartz_GCD");
            AddSubAddon("Quartz", "Quartz_Interrupt");
            AddSubAddon("Quartz", "Quartz_Latency");
            AddSubAddon("Quartz", "Quartz_Mirror");
            AddSubAddon("Quartz", "Quartz_Pet");
            AddSubAddon("Quartz", "Quartz_Player");
            AddSubAddon("Quartz", "Quartz_Range");
            AddSubAddon("Quartz", "Quartz_Swing");
            AddSubAddon("Quartz", "Quartz_Target");
            AddSubAddon("Quartz", "Quartz_Timer");
            AddSubAddon("Quartz", "Quartz_Tradeskill");
            // sctd
            AddSubAddon("sctd", "sctd_options");
            // SW_Stats
            AddSubAddon("SW_Stats", "SW_Stats_Profiles");
            AddSubAddon("SW_Stats", "SW_UniLog");
            // Venantes
            AddSubAddon("Venantes", "VenantesOptions");
            AddSubAddon("Venantes", "SphereLoader");
            // WIM
            AddSubAddon("WIM", "WIM_Options");
            // XPerl
            AddSubAddon("XPerl", "XPerl_ArcaneBar");
            AddSubAddon("XPerl", "XPerl_CustomHighlight");
            AddSubAddon("XPerl", "XPerl_Options");
            AddSubAddon("XPerl", "XPerl_Party");
            AddSubAddon("XPerl", "XPerl_PartyPet");
            AddSubAddon("XPerl", "XPerl_Player");
            AddSubAddon("XPerl", "XPerl_PlayerBuffs");
            AddSubAddon("XPerl", "XPerl_PlayerPet");
            AddSubAddon("XPerl", "XPerl_RaidAdmin");
            AddSubAddon("XPerl", "XPerl_RaidFrames");
            AddSubAddon("XPerl", "XPerl_RaidHelper");
            AddSubAddon("XPerl", "XPerl_RaidMonitor");
            AddSubAddon("XPerl", "XPerl_RaidPets");
            AddSubAddon("XPerl", "XPerl_Target");
            AddSubAddon("XPerl", "XPerl_TargetTarget");
            AddSubAddon("XPerl", "XPerl_Tutorial");

            // Depreciated:
            // Vendor -> new AuctionMaster

            //Acheron,acheron,wowace
            //ActionCombat,action-combat,wowace
            //AddonLoader,addon-loader,wowace
            //Analyst,analyst,wowace
            //Automaton,automaton,wowace

            // TODO: Mappings
            //Accountant
            //ActionBarSaver
            //Afflicted
            //Align
            //Altoholic
            //ArenaHistorian
            //AtlasLootFu
            //Attrition
            //Auc-Advanced
            //Auc-Filter-Basic
            //Auc-ScanData
            //Auc-Stat-Classic
            //Auc-Stat-Histogram
            //Auc-Stat-Purchased
            //Auc-Stat-Simple
            //Auc-Stat-StdDev
            //AuldLangSye
            //Babble-3.3
            //Bagsy
            //BangArtOfWar
            //BeanCounter
            //Bejeweled
            //Broker_Bags
            //Broker_CalendarEvents
            //Broker_Durability
            //Broker_Group
            //Broker_Money
            //Broker_Professions
            //Broker_PvP
            //Broker_Recount
            //Broker_Regen
            //Broker_SysMon
            //Broker_Tracking
            //BtmScan
            //buffet
            //Butsu
            //ButtonBin
            //ButtonFacade_DsmFade-v141
            //ButtonFacade_ItemRack
            //ButtonFacade_LayerTest
            //ButtonFacade_Trinity
            //CandyBar
            //cargoHonor
            //Cartographer3
            //Cartographer3_InstancePOIs
            //Cartographer3_Notes
            //Cartographer3_Waypoints
            //CastYeller
            //CCTracker
            //ChatBar
            //ChatThrottleLib
            //Chinchilla
            //ClassLoot
            //Combuctor_Config
            //Combuctor_Sets
            //Comix
            //CreatureComforts
            //CursorCastbar
            //Dominos
            //Dominos_Buff
            //Dominos_Cast
            //Dominos_Config
            //Dominos_Roll
            //Dominos_XP
            //DoTimer
            //DoTimer_Options
            //EasyMother
            //eCastingBar
            //Enchantrix
            //Enchantrix-Barker
            //Engravings
            //EnhTooltip
            //epgp
            //ExpressMail
            //Fane
            //FeedMachine
            //Fizzle
            //FloatingFrames
            //fontain
            //FuBar_AlchemyFu
            //FuBar_HealBotFu
            //FuBar_ItemRackFu
            //FuBar_LastPlayedFu
            //Fubar_MacroFu
            //FuBar_MailExpiryFu
            //FuBar_MailFu
            //Fubar_ProfessionsBookFu
            //FuBar_WindFuryFu
            //FuBarPlugin-2.0
            //GatherHud
            //GFW_FactionFriend
            //GridLayoutPlus
            //GridStatusHotStack
            //GridStatusIncomingHeals
            //GridStatusLifebloom
            //Haggler
            //HealBot
            //Heatsink
            //HitsMode
            //HKCounter
            //IceHUD
            //IHML
            //Incubator
            //Informant
            //ItemDB
            //JebusMail
            //Junkie
            //KC_Items
            //KillLog
            //LannySimpleMicroMenuFu
            //LearningAid
            //lern2count
            //LibAbacus-3.0
            //LibAboutPanel
            //LibBabble-Boss-3.0
            //LibBabble-Class-3.0
            //LibBabble-Faction-3.0
            //LibBabble-Inventory-3.0
            //LibBabble-Zone-3.0
            //LibBanzai-2.0
            //LibCrayon-3.0
            //LibDogTag-3.0
            //LibDogTag-Items-3.0
            //LibDogTag-Unit-3.0
            //LibFilter-1.0
            //LibFuBarPlugin-3.0
            //LibGraph-2.0
            //LibGratuity-3.0
            //LibGuildPositions-1.0
            //LibHealComm-3.0
            //LibItemBonus-2.0
            //LibJostle-3.0
            //LibPeriodicTable-3.1
            //LibPeriodicTable-3.1-ClassSpell
            //LibPeriodicTable-3.1-Consumable
            //LibPeriodicTable-3.1-Gear
            //LibPeriodicTable-3.1-GearSet
            //LibPeriodicTable-3.1-InstanceLoot
            //LibPeriodicTable-3.1-InstanceLootHeroic
            //LibPeriodicTable-3.1-Misc
            //LibPeriodicTable-3.1-Reputation
            //LibPeriodicTable-3.1-Tradeskill
            //LibPeriodicTable-3.1-TradeskillResultMats
            //LibRangeCheck-2.0
            //LibRock-1.0
            //LibRockComm-1.0
            //LibRockConfig-1.0
            //LibRockConsole-1.0
            //LibRockDB-1.0
            //LibRockEvent-1.0
            //LibRockHook-1.0
            //LibRockLocale-1.0
            //LibRockModuleCore-1.0
            //LibRockTimer-1.0
            //LibRollCall2.0
            //LibSharedMedia-3.0
            //LibSimpleOptions-1.0
            //LibSimpleTimer-1.0
            //LibSink-2.0
            //libtooltip
            //LibTooltip-1.0
            //LibTourist-3.0
            //LinkWrangler
            //LuckyCharms2
            //MacaroonBound
            //MacaroonCB
            //MacaroonXP
            //MacaroonXtras
            //MaterialsTracker
            //Minimalisque_v3
            //Minimalist_ButtonFrame
            //Minimalist_ButtonFrameTitanPlugin
            //MinimapButtonFrame
            //MinimapButtonFrame_SkinPack
            //MinimapButtonFrameFu
            //MinimapButtonFrameTitanPlugin
            //Mirror
            //MonkeyBuddy
            //MonkeyLibrary
            //MonkeyQuest
            //MonkeyQuestLog
            //MoveAnything
            //naiCombo
            //NowCarrying
            //NugEnergy
            //oGlow
            //OneClickBuyOut
            //Overachiever
            //PartySpotter
            //picoDPS
            //picoEXP
            //picoFriends
            //picoGuild
            //PoliteWhisper
            //PreformAvEnabler
            //pRogue
            //QuesterJester
            //RaidBuffStatus
            //RangeDisplay
            //RosterLib
            //Scorecard
            //SellFish
            //ShockAndAwe
            //SickOfClickingDailies
            //SimpleBuffBars
            //SimpleSelfRebuff
            //SimpleSelfRebuff_CastBinding
            //SimpleSelfRebuff_DataObject
            //SimpleSelfRebuff_ItemBuffs
            //SimpleSelfRebuff_Reminder
            //SlashConflict
            //SLDataText
            //SmartBuff
            //SmartMount
            //Snoopy
            //SpecialEvents-Aura-2.0
            //SpecialEvents-Bags-2.0
            //SpecialEventsEmbed
            //SpecialEvents-Equipped-2.0
            //SpecialEvents-LearnSpell-2.0
            //SpecialEvents-Loot-1.0
            //SpecialEvents-Mail-2.0
            //SpecialEvents-Mount-2.0
            //SpecialEvents-Movement-2.0
            //Spyglass
            //SSPVP3
            //StarTip
            //StatBlock_Ammo
            //StatBlock_DPS
            //StatBlock_Factions
            //StatBlock_Money
            //StatBlock_RaidDPS
            //StatBlock_ZoneText
            //StatLogicLib
            //Stubby
            //TankWarnings
            //teksLoot
            //TipTac
            //TipTacOptions
            //TipTacTalents
            //TipTop
            //TotemTimers
            //UnderHood
            //UrbanAchiever
            //VendorBait
            //Violation
            //VuhDo
            //WeaponRebuff
            //WhoLib
            //WindowLib

            // Save File
            _xmlDoc.Save(_xmlFile);
        }
    }
}
