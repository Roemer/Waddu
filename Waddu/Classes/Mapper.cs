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

        private void AddMapping(Mapping newMapping)
        {
            XmlElement addonElement = FindAddonElement(newMapping.AddonName, true);
            XmlElement mappingsElement = FindElement(addonElement, "Mappings", true);

            // Mapping Element
            XmlElement mappingElement = _xmlDoc.CreateElement("Mapping");
            // Site Attribute
            XmlAttribute siteAttribute = _xmlDoc.CreateAttribute("Site");
            siteAttribute.Value = newMapping.AddonSiteId.ToString();
            mappingElement.Attributes.Append(siteAttribute);
            // Tag Attribute
            XmlAttribute tagAttribute = _xmlDoc.CreateAttribute("Tag");
            tagAttribute.Value = newMapping.AddonTag;
            mappingElement.Attributes.Append(tagAttribute);
            // Append Mapping to Mappings
            mappingsElement.AppendChild(mappingElement);
        }

        private void FillInitialMappings()
        {
            // wowAce
            AddMapping(new Mapping("!BugGrabber", "bug-grabber", AddonSiteId.wowace));
            AddMapping(new Mapping("!SurfaceControl", "surface-control", AddonSiteId.wowace));
            AddMapping(new Mapping("Ace2", "ace2", AddonSiteId.wowace));
            AddMapping(new Mapping("Ace3", "ace3", AddonSiteId.wowace));
            AddMapping(new Mapping("ACP", "acp", AddonSiteId.wowace));
            AddMapping(new Mapping("ag_UnitFrames", "ag_unitframes", AddonSiteId.wowace));
            AddMapping(new Mapping("AHsearch", "ahsearch", AddonSiteId.wowace));
            AddMapping(new Mapping("Aloft", "aloft", AddonSiteId.wowace));
            AddMapping(new Mapping("ampere", "ampere", AddonSiteId.wowace));
            AddMapping(new Mapping("Antagonist", "antagonist", AddonSiteId.wowace));
            AddMapping(new Mapping("ArenaPointer", "arena-pointer", AddonSiteId.wowace));
            AddMapping(new Mapping("ArkInventory", "ark-inventory", AddonSiteId.wowace));
            AddMapping(new Mapping("AtlasLoot", "atlasloot-enhanced", AddonSiteId.wowace));
            AddMapping(new Mapping("aUF_Banzai", "a-uf_banzai", AddonSiteId.wowace));
            AddMapping(new Mapping("BadBoy", "bad-boy", AddonSiteId.wowace));
            AddMapping(new Mapping("Baggins", "baggins", AddonSiteId.wowace));
            AddMapping(new Mapping("Baggins_ClosetGnome", "baggins_closet-gnome", AddonSiteId.wowace));
            AddMapping(new Mapping("Baggins_Outfitter", "baggins_outfitter", AddonSiteId.wowace));
            AddMapping(new Mapping("Baggins_Professions", "baggins_professions", AddonSiteId.wowace));
            AddMapping(new Mapping("Baggins_Search", "baggins_search", AddonSiteId.wowace));
            AddMapping(new Mapping("Baggins_SectionColor", "baggins_section-color", AddonSiteId.wowace));
            AddMapping(new Mapping("Baggins_Usable", "baggins_usable", AddonSiteId.wowace));
            AddMapping(new Mapping("BankItems", "bank-items", AddonSiteId.wowace));
            AddMapping(new Mapping("BankStack", "bank-stack", AddonSiteId.wowace));
            AddMapping(new Mapping("Bartender4", "bartender4", AddonSiteId.wowace));
            AddMapping(new Mapping("BasicBuffs", "basic-buffs", AddonSiteId.wowace));
            AddMapping(new Mapping("beql", "beql", AddonSiteId.wowace));
            AddMapping(new Mapping("BetterInbox", "better-inbox", AddonSiteId.wowace));
            AddMapping(new Mapping("BigBrother", "big-brother", AddonSiteId.wowace));
            AddMapping(new Mapping("BigWigs", "big-wigs", AddonSiteId.wowace));
            AddMapping(new Mapping("BigWigs_KalecgosHealth", "bwkh", AddonSiteId.wowace));
            AddMapping(new Mapping("Broker_Clock", "broker_clock", AddonSiteId.wowace));
            AddMapping(new Mapping("Broker_Mail", "broker_mail", AddonSiteId.wowace));
            AddMapping(new Mapping("Broker2FuBar", "broker2fubar", AddonSiteId.wowace));
            AddMapping(new Mapping("Buffalo", "buffalo", AddonSiteId.wowace));
            AddMapping(new Mapping("BuffEnough", "buff-enough", AddonSiteId.wowace));
            AddMapping(new Mapping("BugSack", "bugsack", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade", "buttonfacade", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade_Apathy", "buttonfacade_apathy", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade_Caith", "buttonfacade_caith", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade_Entropy", "buttonfacade_entropy", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade_Gears", "buttonfacade_gears", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade_HKitty", "button-facade_hkitty", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade_LiteStep", "buttonfacade_litestep", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade_Onyx", "buttonfacade_onyx", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade_Serenity", "buttonfacade_serenity", AddonSiteId.wowace));
            AddMapping(new Mapping("ButtonFacade_simpleSquare", "button-facade_simple-square", AddonSiteId.wowace));
            AddMapping(new Mapping("Capping", "capping", AddonSiteId.wowace));
            AddMapping(new Mapping("Cartographer", "cartographer", AddonSiteId.wowace));
            AddMapping(new Mapping("CCBreaker", "ccbreaker", AddonSiteId.wowace));
            AddMapping(new Mapping("Chaman2", "chaman2", AddonSiteId.wowace));
            AddMapping(new Mapping("Chatter", "chatter", AddonSiteId.wowace));
            AddMapping(new Mapping("Chinchilla", "chinchilla", AddonSiteId.wowace));
            AddMapping(new Mapping("ClassTimer", "classtimer", AddonSiteId.wowace));
            AddMapping(new Mapping("ClearFont2", "clear-font2", AddonSiteId.wowace));
            AddMapping(new Mapping("ClearFont2_FontPack", "clear-font2_font-pack", AddonSiteId.wowace));
            AddMapping(new Mapping("Click2Cast", "click2cast", AddonSiteId.wowace));
            AddMapping(new Mapping("ClosetGnome", "closet-gnome", AddonSiteId.wowace));
            AddMapping(new Mapping("ClosetGnome_Banker", "closet-gnome_banker", AddonSiteId.wowace));
            AddMapping(new Mapping("ClosetGnome_HelmNCloak", "closet-gnome_helm-ncloak", AddonSiteId.wowace));
            AddMapping(new Mapping("ClosetGnome_Mount", "closet-gnome_mount", AddonSiteId.wowace));
            AddMapping(new Mapping("ClosetGnome_Tooltip", "closet-gnome_tooltip", AddonSiteId.wowace));
            AddMapping(new Mapping("Coconuts", "coconuts", AddonSiteId.wowace));
            AddMapping(new Mapping("Combuctor", "combuctor", AddonSiteId.wowace));
            AddMapping(new Mapping("CooldownCount", "cooldown-count", AddonSiteId.wowace));
            AddMapping(new Mapping("CooldownTimers2", "cooldown-timers2", AddonSiteId.wowace));
            AddMapping(new Mapping("CowTip", "cowtip", AddonSiteId.wowace));
            AddMapping(new Mapping("Cryolysis2", "cryolysis2", AddonSiteId.wowace));
            AddMapping(new Mapping("Damnation", "damnation", AddonSiteId.wowace));
            AddMapping(new Mapping("DBM_API", "deadly-boss-mods", AddonSiteId.wowace));
            AddMapping(new Mapping("Dock", "dock", AddonSiteId.wowace));
            AddMapping(new Mapping("DotDotDot", "dot-dot-dot", AddonSiteId.wowace));
            AddMapping(new Mapping("DrDamage", "dr-damage", AddonSiteId.wowace));
            AddMapping(new Mapping("EavesDrop", "eaves-drop", AddonSiteId.wowace));
            AddMapping(new Mapping("eePanels2", "ee-panels2", AddonSiteId.wowace));
            AddMapping(new Mapping("ElkBuffBars", "elkbuffbars", AddonSiteId.wowace));
            AddMapping(new Mapping("EQCompare", "eqcompare", AddonSiteId.wowace));
            AddMapping(new Mapping("ErrorMonster", "error-monster", AddonSiteId.wowace));
            AddMapping(new Mapping("Fence", "fence", AddonSiteId.wowace));
            AddMapping(new Mapping("FishermansFriend", "fishermans-friend", AddonSiteId.wowace));
            AddMapping(new Mapping("Fortress", "fortress", AddonSiteId.wowace));
            AddMapping(new Mapping("FreeRefills", "free-refills", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar", "fubar", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_AmmoFu", "fubar_ammofu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_BagFu", "fu-bar_bag-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_ClockFu", "fubar_clockfu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_DurabilityFu", "fubar_durabilityfu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_DuraTek", "fu-bar_dura-tek", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_ExperienceFu", "fubar_experiencefu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_FactionsFu", "fubar_factionsfu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_FriendsFu", "fubar_friendsfu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_FuXPFu", "fu-bar_fu-xpfu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_GarbageFu", "fu-bar_garbage-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_GroupFu", "fubar_groupfu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_GuildFu", "fubar_guildfu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_HonorFu", "fubar_honorfu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_ItemBonusesFu", "fu-bar_item-bonuses-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_LocationFu", "fubar_locationfu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_MicroMenuFu", "fubar_micromenufu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_MoneyFu", "fu-bar_money-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_NameToggleFu", "fubar_nametogglefu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_PerformanceFu", "fubar_performancefu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_PetFu", "fu-bar_pet-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_QuestsFu", "fu-bar_quests-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_RaidBuffFu", "fu-bar_raid-buff-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_RecountFu", "fu-bar_recount-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_ReagentFu", "fu-bar_reagent-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_SkillsPlusFu", "fu-bar_skills-plus-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_SpeedFu", "fu-bar_speed-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_ToFu", "fu-bar_to-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_TopScoreFu", "fu-bar_top-score-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_TrainerFu", "fu-bar_trainer-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_TransporterFu", "fu-bar_transporter-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar_VolumeFu", "fu-bar_volume-fu", AddonSiteId.wowace));
            AddMapping(new Mapping("FuBar2Broker", "fubar2broker", AddonSiteId.wowace));
            AddMapping(new Mapping("FuTextures", "futextures", AddonSiteId.wowace));
            AddMapping(new Mapping("GatherMate", "gathermate", AddonSiteId.wowace));
            AddMapping(new Mapping("GatherMate_Data", "gathermate_data", AddonSiteId.wowace));
            AddMapping(new Mapping("GatherMate_Sharing", "gathermate_sharing", AddonSiteId.wowace));
            AddMapping(new Mapping("GloryLib", "glorylib", AddonSiteId.wowace));
            AddMapping(new Mapping("Grid", "grid", AddonSiteId.wowace));
            AddMapping(new Mapping("GridIndicatorSideIcons", "grid-indicator-side-icons", AddonSiteId.wowace));
            AddMapping(new Mapping("GridIndicatorText3", "grid-indicator-text3", AddonSiteId.wowace));
            AddMapping(new Mapping("GridManaBars", "grid-mana-bars", AddonSiteId.wowace));
            AddMapping(new Mapping("GridSideIndicators", "grid-side-indicators", AddonSiteId.wowace));
            AddMapping(new Mapping("GridStatusAFK", "grid-status-afk", AddonSiteId.wowace));
            AddMapping(new Mapping("GridStatusHots", "grid-status-hots", AddonSiteId.wowace));
            AddMapping(new Mapping("GridStatusMissingBuffs", "grid-status-missing-buffs", AddonSiteId.wowace));
            AddMapping(new Mapping("GridStatusRaidIcons", "grid-status-raid-icons", AddonSiteId.wowace));
            AddMapping(new Mapping("GridStatusReadyCheck", "grid-status-ready-check", AddonSiteId.wowace));
            AddMapping(new Mapping("Grid2", "grid2", AddonSiteId.wowace));
            AddMapping(new Mapping("GrimReaper", "grim-reaper", AddonSiteId.wowace));
            AddMapping(new Mapping("HandyNotes", "handynotes", AddonSiteId.wowace));
            AddMapping(new Mapping("HandyNotes_FlightMasters", "handy-notes_flight-masters", AddonSiteId.wowace));
            AddMapping(new Mapping("HandyNotes_Guild", "handynotes_guild", AddonSiteId.wowace));
            AddMapping(new Mapping("HandyNotes_Mailboxes", "handy-notes_mailboxes", AddonSiteId.wowace));
            AddMapping(new Mapping("HeadCount", "head-count", AddonSiteId.wowace));
            AddMapping(new Mapping("InstanceMaps", "instance-maps", AddonSiteId.wowace));
            AddMapping(new Mapping("ItemPriceTooltip", "item-price-tooltip", AddonSiteId.wowace));
            AddMapping(new Mapping("kgPanels", "kg-panels", AddonSiteId.wowace));
            AddMapping(new Mapping("LD50_abar", "ld50_abar", AddonSiteId.wowace));
            AddMapping(new Mapping("Links", "links", AddonSiteId.wowace));
            AddMapping(new Mapping("LittleWigs", "little-wigs", AddonSiteId.wowace));
            AddMapping(new Mapping("MagicDKP", "magic-dk", AddonSiteId.wowace));
            AddMapping(new Mapping("MainAssist", "main-assist", AddonSiteId.wowace));
            AddMapping(new Mapping("Manufac", "manufac", AddonSiteId.wowace));
            AddMapping(new Mapping("Mapster", "mapster", AddonSiteId.wowace));
            AddMapping(new Mapping("MBB", "mbb", AddonSiteId.wowace));
            AddMapping(new Mapping("Mendeleev", "mendeleev", AddonSiteId.wowace));
            AddMapping(new Mapping("Minimalist", "minimalist", AddonSiteId.wowace));
            AddMapping(new Mapping("MiniMapster", "minimapster", AddonSiteId.wowace));
            AddMapping(new Mapping("MobSpells", "mob-spells", AddonSiteId.wowace));
            AddMapping(new Mapping("MobHealth", "mob-health", AddonSiteId.wowace));
            AddMapping(new Mapping("MobHealth3_BlizzardFrames", "mob-health3_blizzard-frames", AddonSiteId.wowace));
            AddMapping(new Mapping("Niagara", "niagara", AddonSiteId.wowace));
            AddMapping(new Mapping("Numen", "numen", AddonSiteId.wowace));
            AddMapping(new Mapping("nQuestLog", "n-quest-log", AddonSiteId.wowace));
            AddMapping(new Mapping("Omen", "omen-threat-meter", AddonSiteId.wowace));
            AddMapping(new Mapping("OneBag3", "onebag3", AddonSiteId.wowace));
            AddMapping(new Mapping("OptiTaunt", "opti-taunt", AddonSiteId.wowace));
            AddMapping(new Mapping("oRA2", "ora2", AddonSiteId.wowace));
            AddMapping(new Mapping("Parrot", "parrot", AddonSiteId.wowace));
            AddMapping(new Mapping("PitBull", "pit-bull", AddonSiteId.wowace));
            AddMapping(new Mapping("PitBull_QuickHealth", "pit-bull_quick-health", AddonSiteId.wowace));
            AddMapping(new Mapping("Postal", "postal", AddonSiteId.wowace));
            AddMapping(new Mapping("Prat-3.0", "prat-3-0", AddonSiteId.wowace));
            AddMapping(new Mapping("ProfessionsBook", "professions-book", AddonSiteId.wowace));
            AddMapping(new Mapping("Proximo", "proximo", AddonSiteId.wowace));
            AddMapping(new Mapping("Quartz", "quartz", AddonSiteId.wowace));
            AddMapping(new Mapping("Quixote", "quixote", AddonSiteId.wowace));
            AddMapping(new Mapping("RangeRecolor", "range-recolor", AddonSiteId.wowace));
            AddMapping(new Mapping("RatingBuster", "rating-buster", AddonSiteId.wowace));
            AddMapping(new Mapping("Recount", "recount", AddonSiteId.wowace));
            AddMapping(new Mapping("Rep2", "rep2", AddonSiteId.wowace));
            AddMapping(new Mapping("Reslack", "reslack", AddonSiteId.wowace));
            AddMapping(new Mapping("RightWing", "right-wing", AddonSiteId.wowace));
            AddMapping(new Mapping("Routes", "routes", AddonSiteId.wowace));
            AddMapping(new Mapping("rSelfCast", "r-self-cast", AddonSiteId.wowace));
            AddMapping(new Mapping("SalvationKiller", "salvation-killer", AddonSiteId.wowace));
            AddMapping(new Mapping("sct", "sct", AddonSiteId.wowace));
            AddMapping(new Mapping("sct_options", "sct_options", AddonSiteId.wowace));
            AddMapping(new Mapping("SharedMedia", "sharedmedia", AddonSiteId.wowace));
            AddMapping(new Mapping("SharedMediaAdditionalFonts", "shared-media-additional-fonts", AddonSiteId.wowace));
            AddMapping(new Mapping("simpleMinimap", "simple-minimap", AddonSiteId.wowace));
            AddMapping(new Mapping("Skillet", "skillet", AddonSiteId.wowace));
            AddMapping(new Mapping("Skinner", "skinner", AddonSiteId.wowace));
            AddMapping(new Mapping("SnapshotBar", "snapshot-bar", AddonSiteId.wowace));
            AddMapping(new Mapping("Sphere", "sphere", AddonSiteId.wowace));
            AddMapping(new Mapping("sRaidFrames", "sraidframes", AddonSiteId.wowace));
            AddMapping(new Mapping("StatBlockCore", "stat-block-core", AddonSiteId.wowace));
            AddMapping(new Mapping("StatBlock_Durability", "stat-block_durability", AddonSiteId.wowace));
            AddMapping(new Mapping("StatBlock_FPS", "stat-block_fps", AddonSiteId.wowace));
            AddMapping(new Mapping("StatBlock_Latency", "stat-block_latency", AddonSiteId.wowace));
            AddMapping(new Mapping("StatBlock_Memory", "stat-block_memory", AddonSiteId.wowace));
            AddMapping(new Mapping("Talented", "talented", AddonSiteId.wowace));
            AddMapping(new Mapping("Talented_Data", "talented_data", AddonSiteId.wowace));
            AddMapping(new Mapping("Talented_Loader", "talented_loader", AddonSiteId.wowace));
            AddMapping(new Mapping("TouristLib", "touristlib", AddonSiteId.wowace));
            AddMapping(new Mapping("Transcriptor", "transcriptor", AddonSiteId.wowace));
            AddMapping(new Mapping("WitchHunt", "witch-hunt", AddonSiteId.wowace));
            AddMapping(new Mapping("WoWEquip", "wowequip", AddonSiteId.wowace));
            AddMapping(new Mapping("XLoot", "xloot", AddonSiteId.wowace));
            AddMapping(new Mapping("XLootGroup", "xloot-group", AddonSiteId.wowace));
            AddMapping(new Mapping("XLootMaster", "xloot-master", AddonSiteId.wowace));
            AddMapping(new Mapping("XLootMonitor", "xloot-monitor", AddonSiteId.wowace));
            AddMapping(new Mapping("XPerl", "xperl", AddonSiteId.wowace));
            AddMapping(new Mapping("XRS", "xrs", AddonSiteId.wowace));
            AddMapping(new Mapping("YurrCombatLog", "yurr-combat-log", AddonSiteId.wowace));
            AddMapping(new Mapping("ZOMGBuffs", "zomgbuffs", AddonSiteId.wowace));

            // Blizzard
            AddMapping(new Mapping("Blizzard_AchievementUI", "Blizzard_AchievementUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_AuctionUI", "Blizzard_AuctionUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_BarbershopUI", "Blizzard_BarbershopUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_BattlefieldMinimap", "Blizzard_BattlefieldMinimap", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_BindingUI", "Blizzard_BindingUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_Calendar", "Blizzard_Calendar", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_CombatLog", "Blizzard_CombatLog", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_CombatText", "Blizzard_CombatText", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_CraftUI", "Blizzard_CraftUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_FeedbackUI", "Blizzard_FeedbackUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_GlyphUI", "Blizzard_GlyphUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_GMSurveyUI", "Blizzard_GMSurveyUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_GuildBankUI", "Blizzard_GuildBankUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_InspectUI", "Blizzard_InspectUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_ItemSocketingUI", "Blizzard_ItemSocketingUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_MacroUI", "Blizzard_MacroUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_RaidUI", "Blizzard_RaidUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_TalentUI", "Blizzard_TalentUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_TimeManager", "Blizzard_TimeManager", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_TokenUI", "Blizzard_TokenUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_TradeSkillUI", "Blizzard_TradeSkillUI", AddonSiteId.blizzard));
            AddMapping(new Mapping("Blizzard_TrainerUI", "Blizzard_TrainerUI", AddonSiteId.blizzard));

            // WoWInterface
            AddMapping(new Mapping("!!Warmup", "4939", AddonSiteId.wowinterface)); // RAR
            AddMapping(new Mapping("!StopTheSpam", "4077", AddonSiteId.wowinterface));
            AddMapping(new Mapping("AckisRecipeList", "8512", AddonSiteId.wowinterface));
            AddMapping(new Mapping("AddonManager", "7164", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Afflicted2", "8063", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Aloft", "10864", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Atlas", "3896", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Bongos", "8419", AddonSiteId.wowinterface));
            AddMapping(new Mapping("ButtonFacade_Elegance", "9623", AddonSiteId.wowinterface));
            AddMapping(new Mapping("ButtonFacade_Sleek", "11010", AddonSiteId.wowinterface));
            AddMapping(new Mapping("ButtonFacade_Tones", "9712", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Capping", "11177", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Clique", "5108", AddonSiteId.wowinterface));
            AddMapping(new Mapping("DPS", "11059", AddonSiteId.wowinterface));
            AddMapping(new Mapping("DRTracker", "8901", AddonSiteId.wowinterface));
            AddMapping(new Mapping("EquipCompare", "4392", AddonSiteId.wowinterface));
            AddMapping(new Mapping("FuBar_AuditorFu", "5429", AddonSiteId.wowinterface));
            AddMapping(new Mapping("GFW_FeedOMatic", "4160", AddonSiteId.wowinterface));
            AddMapping(new Mapping("GhostPulse", "8418", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Hack", "11101", AddonSiteId.wowinterface));
            AddMapping(new Mapping("InFlight", "11178", AddonSiteId.wowinterface));
            AddMapping(new Mapping("ItemRack", "6818", AddonSiteId.wowinterface));
            AddMapping(new Mapping("LitheTooltipDoctor", "5336", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Ludwig", "5174", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Macaroon", "10636", AddonSiteId.wowinterface));
            AddMapping(new Mapping("MikScrollingBattleText", "5153", AddonSiteId.wowinterface));
            AddMapping(new Mapping("MTLove", "7024", AddonSiteId.wowinterface));
            AddMapping(new Mapping("OmniCC", "4836", AddonSiteId.wowinterface));
            AddMapping(new Mapping("OPie", "9094", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Possessions", "6443", AddonSiteId.wowinterface));
            AddMapping(new Mapping("QuestHelper", "9896", AddonSiteId.wowinterface));
            AddMapping(new Mapping("ReagentRestocker", "7569", AddonSiteId.wowinterface));
            AddMapping(new Mapping("Tipachu", "8808", AddonSiteId.wowinterface));
            AddMapping(new Mapping("TomTom", "7032", AddonSiteId.wowinterface));
            AddMapping(new Mapping("TotemManager", "10080", AddonSiteId.wowinterface));
            AddMapping(new Mapping("TourGuide", "7674", AddonSiteId.wowinterface));

            // WoWUI
            AddMapping(new Mapping("Atlas", "400", AddonSiteId.wowui));
            AddMapping(new Mapping("QuestGuru", "4855", AddonSiteId.curse));
            AddMapping(new Mapping("QuestHelper", "6271", AddonSiteId.wowui));
            AddMapping(new Mapping("RogueFocus", "6060", AddonSiteId.wowui));
            AddMapping(new Mapping("Venantes", "4692", AddonSiteId.wowui));

            // Curseforge
            AddMapping(new Mapping("BloodyFont", "bloody-font-2-0", AddonSiteId.curseforge));
            AddMapping(new Mapping("Chaman2", "totem-manager", AddonSiteId.curseforge));
            AddMapping(new Mapping("Decursive", "decursive", AddonSiteId.curseforge));
            AddMapping(new Mapping("QuestHelper", "quest-helper", AddonSiteId.curseforge));
            AddMapping(new Mapping("AuctionMaster", "vendor", AddonSiteId.curseforge)); // Renamed from Vendor

            // Curse
            AddMapping(new Mapping("Atlas", "atlas", AddonSiteId.curse));
            AddMapping(new Mapping("Bagnon", "bagnon", AddonSiteId.curse));
            AddMapping(new Mapping("Carbonite", "carbonite-quest", AddonSiteId.curse));
            AddMapping(new Mapping("DruidBar", "druid-bar", AddonSiteId.curse));
            AddMapping(new Mapping("DynamicPerformance", "dynamic-performance", AddonSiteId.curse));
            AddMapping(new Mapping("Examiner", "examiner", AddonSiteId.curse));
            AddMapping(new Mapping("FlightMap", "flight-map", AddonSiteId.curse));
            AddMapping(new Mapping("FocusFrame", "focus-frame", AddonSiteId.curse));
            AddMapping(new Mapping("GoGoMount", "go-go-mount", AddonSiteId.curse));
            AddMapping(new Mapping("ItemRack", "item-rack", AddonSiteId.curse));
            AddMapping(new Mapping("MikScrollingBattleText", "mik-scrolling-battle-text", AddonSiteId.curse));
            AddMapping(new Mapping("Necrosis", "necrosis-ld-c", AddonSiteId.curse));
            AddMapping(new Mapping("OmniCC", "omni-cc", AddonSiteId.curse));
            AddMapping(new Mapping("Outfitter", "outfitter", AddonSiteId.curse));
            AddMapping(new Mapping("QuestGuru", "quest-guru", AddonSiteId.curse));
            AddMapping(new Mapping("QuestHelper", "quest-helper", AddonSiteId.curse));
            AddMapping(new Mapping("QuickStatsV2", "quick-stats-v2", AddonSiteId.curse));
            AddMapping(new Mapping("sctd", "sct-damage", AddonSiteId.curse));
            AddMapping(new Mapping("SimpleRaidTargetIcons", "simple-raid-target-icons", AddonSiteId.curse));
            AddMapping(new Mapping("SpamMeNot", "spam-me-not", AddonSiteId.curse));
            AddMapping(new Mapping("SW_Stats", "sw-stats", AddonSiteId.curse));
            AddMapping(new Mapping("TrinketMenu", "trinket-menu", AddonSiteId.curse));
            AddMapping(new Mapping("WhoHas", "who-has", AddonSiteId.curse));
            AddMapping(new Mapping("WIM", "wim-wo-w-instant-messenger", AddonSiteId.curse));
            AddMapping(new Mapping("XLoot", "xloot", AddonSiteId.curse));
            AddMapping(new Mapping("ZHunterMod", "zhunter-mod", AddonSiteId.curse));

            // Special
            AddMapping(new Mapping("CT_Core", "CT_Core", AddonSiteId.wowspecial));
            AddMapping(new Mapping("CT_RaidTracker", "CT_RaidTracker", AddonSiteId.wowspecial));
            AddMapping(new Mapping("CT_UnitFrames", "CT_UnitFrames", AddonSiteId.wowspecial));
            AddMapping(new Mapping("MarsPartyBuff", "MarsPartyBuff", AddonSiteId.wowspecial));
            AddMapping(new Mapping("MobMap", "MobMap", AddonSiteId.wowspecial));
            AddMapping(new Mapping("xchar", "xchar", AddonSiteId.wowspecial));
            AddMapping(new Mapping("xchar_Arygos", "xchar_Arygos", AddonSiteId.wowspecial));
            AddMapping(new Mapping("xchar_Kargath", "xchar_Kargath", AddonSiteId.wowspecial));
            AddMapping(new Mapping("xchar_MalGanis", "xchar_MalGanis", AddonSiteId.wowspecial));

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

            // TODO: Mappings
            //BtmScan
            //FuBar_MailFu
            //Informant
            //LannySimpleMicroMenuFu
            //MacaroonBound
            //MacaroonXtras
            //Stubby
            //AtlasLootFu
            //Auc-Advanced
            //Auc-Filter-Basic
            //Auc-ScanData
            //Auc-Stat-Classic
            //Auc-Stat-Histogram
            //Auc-Stat-Purchased
            //Auc-Stat-Simple
            //Auc-Stat-StdDev
            //BeanCounter
            //Enchantrix
            //Enchantrix-Barker
            //EnhTooltip
            //Informant
            //Minimalist_ButtonFrame
            //Minimalist_ButtonFrameTitanPlugin
            //-Shantalya-Infoleiste
            //ActionBarSaver
            //AuldLangSye
            //Babble-3.3
            //Bejeweled
            //Chinchilla
            //CreatureComforts
            //EasyMother
            //Engravings
            //ExpressMail
            //Fubar_MacroFu
            //Fubar_ProfessionsBookFu
            //LibCrayon-3.0
            //LibRockCOnsole-1.0
            //LibRollCall2.0
            //LibTourist-3.0
            //OneClickBuyOut
            //PoliteWhisper
            //PreformAvEnabler

            // Save File
            _xmlDoc.Save(_xmlFile);
        }
    }
}
