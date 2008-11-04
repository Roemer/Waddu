using System.Collections.Generic;
using System.IO;
using System.Xml;
using Waddu.BusinessObjects;
using Waddu.Types;

namespace Waddu.Classes
{
    public class Mapper
    {
        List<Addon> _addonList;
        List<Package> _packageList;

        public Mapper(string mappingFile)
        {
            _addonList = new List<Addon>();
            _packageList = new List<Package>();

            Init();
            SaveXML(mappingFile);
        }

        public static void CreateMapping(string mappingFile)
        {
            Mapper map = new Mapper(mappingFile);
        }

        private void SaveXML(string mappingFile)
        {
            string _xmlFile = mappingFile;
            XmlDocument xmlDoc;
            File.Delete(_xmlFile);
            if (!File.Exists(_xmlFile))
            {
                XmlTextWriter w = new XmlTextWriter(_xmlFile, null);
                w.WriteStartDocument();
                w.WriteStartElement("Waddu_Mappings");

                w.WriteStartElement("Game");
                w.WriteStartAttribute("Name");
                w.WriteString(GameType.ConvertToString(GameType.Enum.WorldOfWarcraft));
                w.WriteEndAttribute();

                foreach (Addon addon in _addonList)
                {
                    if (addon.Mappings.Count == 0)
                    {
                        // Skip SubAddons only
                        continue;
                    }

                    w.WriteStartElement("Addon");
                    w.WriteStartAttribute("Name");
                    w.WriteString(addon.Name);
                    w.WriteEndAttribute();

                    if (addon.Mappings.Count > 0)
                    {
                        w.WriteStartElement("Mappings");
                        foreach (Mapping map in addon.Mappings)
                        {
                            w.WriteStartElement("Mapping");
                            w.WriteStartAttribute("Site");
                            w.WriteString(map.AddonSiteId.ToString());
                            w.WriteEndAttribute();
                            w.WriteStartAttribute("Tag");
                            w.WriteString(map.AddonTag);
                            w.WriteEndAttribute();
                            w.WriteEndElement();
                        }
                        w.WriteEndElement();
                    }

                    if (addon.SubAddons.Count > 0)
                    {
                        w.WriteStartElement("SubAddons");
                        foreach (Addon subAddon in addon.SubAddons)
                        {
                            w.WriteStartElement("SubAddon");
                            w.WriteStartAttribute("Name");
                            w.WriteString(subAddon.Name);
                            w.WriteEndAttribute();
                            w.WriteEndElement();
                        }
                        w.WriteEndElement();
                    }

                    /*if (addon.SuperAddons.Count > 0)
                    {
                        w.WriteStartElement("SuperAddons");
                        foreach (Addon superAddon in addon.SuperAddons)
                        {
                            w.WriteStartElement("SuperAddon");
                            w.WriteStartAttribute("Name");
                            w.WriteString(superAddon.Name);
                            w.WriteEndAttribute();
                            w.WriteEndElement();
                        }
                        w.WriteEndElement();
                    }*/

                    /*if (addon.Packages.Count > 0)
                    {
                        w.WriteStartElement("Packages");
                        foreach (Package package in addon.Packages)
                        {
                            w.WriteStartElement("Package");
                            w.WriteStartAttribute("Name");
                            w.WriteString(package.Name);
                            w.WriteEndAttribute();
                            w.WriteEndElement();
                        }
                        w.WriteEndElement();
                    }*/

                    w.WriteEndElement();
                }

                foreach (Package package in _packageList)
                {
                    if (package.Mappings.Count == 0)
                    {
                        // Skip Packages without Mappings
                        continue;
                    }

                    w.WriteStartElement("Package");
                    w.WriteStartAttribute("Name");
                    w.WriteString(package.Name);
                    w.WriteEndAttribute();

                    if (package.Mappings.Count > 0)
                    {
                        w.WriteStartElement("Mappings");
                        foreach (Mapping map in package.Mappings)
                        {
                            w.WriteStartElement("Mapping");
                            w.WriteStartAttribute("Site");
                            w.WriteString(map.AddonSiteId.ToString());
                            w.WriteEndAttribute();
                            w.WriteStartAttribute("Tag");
                            w.WriteString(map.AddonTag);
                            w.WriteEndAttribute();
                            w.WriteEndElement();
                        }
                        w.WriteEndElement();
                    }

                    if (package.Addons.Count > 0)
                    {
                        w.WriteStartElement("Addons");
                        foreach (Addon addon in package.Addons)
                        {
                            w.WriteStartElement("Addon");
                            w.WriteStartAttribute("Name");
                            w.WriteString(addon.Name);
                            w.WriteEndAttribute();
                            w.WriteEndElement();
                        }
                        w.WriteEndElement();
                    }

                    w.WriteEndElement();
                }

                w.WriteEndElement();
                w.WriteEndElement();
                w.WriteEndDocument();
                w.Close();
            }

            xmlDoc = new XmlDocument();
            xmlDoc.Load(_xmlFile);

            xmlDoc.Save(_xmlFile);
        }

        private Addon GetAddon(string addonName)
        {
            Addon addon = _addonList.Find(delegate(Addon o) { return o.Name.Equals(addonName); });
            if (addon == null)
            {
                addon = new Addon(addonName);
                _addonList.Add(addon);
            }
            return addon;
        }

        private Package GetPackage(string packageName)
        {
            Package package = _packageList.Find(delegate(Package o) { return o.Name.Equals(packageName); });
            if (package == null)
            {
                package = new Package(packageName);
                _packageList.Add(package);
            }
            return package;
        }

        private void Init()
        {
            GetAddon("!!Warmup").Mappings.Add(new Mapping("4939", AddonSiteId.wowinterface));
            GetAddon("!BugGrabber").Mappings.Add(new Mapping("bug-grabber", AddonSiteId.curse));
            GetAddon("!BugGrabber").Mappings.Add(new Mapping("bug-grabber", AddonSiteId.wowace));
            GetAddon("!StopTheSpam").Mappings.Add(new Mapping("4077", AddonSiteId.wowinterface));
            GetAddon("!SurfaceControl").Mappings.Add(new Mapping("surface-control", AddonSiteId.curse));
            GetAddon("!SurfaceControl").Mappings.Add(new Mapping("surface-control", AddonSiteId.wowace));
            GetAddon("Accountant").Mappings.Add(new Mapping("3733", AddonSiteId.wowui));
            GetAddon("Accountant").Mappings.Add(new Mapping("accountant-updated", AddonSiteId.curse));
            GetAddon("Ace").Mappings.Add(new Mapping("ace", AddonSiteId.curse));
            GetAddon("Ace").Mappings.Add(new Mapping("ace", AddonSiteId.wowace));
            GetAddon("Ace2").Mappings.Add(new Mapping("ace2", AddonSiteId.curse));
            GetAddon("Ace2").Mappings.Add(new Mapping("ace2", AddonSiteId.wowace));
            GetAddon("Ace3").Mappings.Add(new Mapping("ace3", AddonSiteId.curse));
            GetAddon("Ace3").Mappings.Add(new Mapping("ace3", AddonSiteId.wowace));
            GetAddon("Acheron").Mappings.Add(new Mapping("acheron", AddonSiteId.curse));
            GetAddon("Acheron").Mappings.Add(new Mapping("acheron", AddonSiteId.wowace));
            GetAddon("AckisRecipeList").Mappings.Add(new Mapping("8512", AddonSiteId.wowinterface));
            GetAddon("ACP").Mappings.Add(new Mapping("acp", AddonSiteId.curse));
            GetAddon("ACP").Mappings.Add(new Mapping("acp", AddonSiteId.wowace));
            GetAddon("ActionBarSaver").Mappings.Add(new Mapping("8075", AddonSiteId.wowinterface));
            GetAddon("ActionCombat").Mappings.Add(new Mapping("action-combat", AddonSiteId.curse));
            GetAddon("ActionCombat").Mappings.Add(new Mapping("action-combat", AddonSiteId.wowace));
            GetAddon("AddonLoader").Mappings.Add(new Mapping("addon-loader", AddonSiteId.curse));
            GetAddon("AddonLoader").Mappings.Add(new Mapping("addon-loader", AddonSiteId.wowace));
            GetAddon("AddonManager").Mappings.Add(new Mapping("7164", AddonSiteId.wowinterface));
            GetAddon("Afflicted2").Mappings.Add(new Mapping("8063", AddonSiteId.wowinterface));
            GetAddon("ag_UnitFrames").Mappings.Add(new Mapping("ag_unitframes", AddonSiteId.curse));
            GetAddon("ag_UnitFrames").Mappings.Add(new Mapping("ag_unitframes", AddonSiteId.wowace));
            GetAddon("ag_UnitFrames").SubAddons.Add(GetAddon("ag_Extras"));
            GetAddon("ag_UnitFrames").SubAddons.Add(GetAddon("ag_Options"));
            GetAddon("AHsearch").Mappings.Add(new Mapping("ahsearch", AddonSiteId.curse));
            GetAddon("AHsearch").Mappings.Add(new Mapping("ahsearch", AddonSiteId.wowace));
            GetAddon("Align").Mappings.Add(new Mapping("6153", AddonSiteId.wowinterface));
            GetAddon("Aloft").Mappings.Add(new Mapping("10864", AddonSiteId.wowinterface));
            GetAddon("Aloft").Mappings.Add(new Mapping("aloft", AddonSiteId.curse));
            GetAddon("Aloft").Mappings.Add(new Mapping("aloft", AddonSiteId.wowace));
            GetAddon("Altoholic").Mappings.Add(new Mapping("8533", AddonSiteId.wowinterface));
            GetAddon("Altoholic").Mappings.Add(new Mapping("altoholic", AddonSiteId.curse));
            GetAddon("ampere").Mappings.Add(new Mapping("ampere", AddonSiteId.curse));
            GetAddon("ampere").Mappings.Add(new Mapping("ampere", AddonSiteId.wowace));
            GetAddon("Analyst").Mappings.Add(new Mapping("analyst", AddonSiteId.curse));
            GetAddon("Analyst").Mappings.Add(new Mapping("analyst", AddonSiteId.wowace));
            GetAddon("Antagonist").Mappings.Add(new Mapping("antagonist", AddonSiteId.curse));
            GetAddon("Antagonist").Mappings.Add(new Mapping("antagonist", AddonSiteId.wowace));
            GetAddon("ArenaHistorian").Mappings.Add(new Mapping("8224", AddonSiteId.wowinterface));
            GetAddon("ArenaPointer").Mappings.Add(new Mapping("arena-pointer", AddonSiteId.curse));
            GetAddon("ArenaPointer").Mappings.Add(new Mapping("arena-pointer", AddonSiteId.wowace));
            GetAddon("ArkInventory").Mappings.Add(new Mapping("ark-inventory", AddonSiteId.curse));
            GetAddon("ArkInventory").Mappings.Add(new Mapping("ark-inventory", AddonSiteId.wowace));
            GetAddon("Atlas").Mappings.Add(new Mapping("3896", AddonSiteId.wowinterface));
            GetAddon("Atlas").Mappings.Add(new Mapping("400", AddonSiteId.wowui));
            GetAddon("Atlas").Mappings.Add(new Mapping("atlas", AddonSiteId.curse));
            GetAddon("Atlas").SubAddons.Add(GetAddon("Atlas_Battlegrounds"));
            GetAddon("Atlas").SubAddons.Add(GetAddon("Atlas_DungeonLocs"));
            GetAddon("Atlas").SubAddons.Add(GetAddon("Atlas_FlightPaths"));
            GetAddon("Atlas").SubAddons.Add(GetAddon("Atlas_OutdoorRaids"));
            GetAddon("AtlasLoot").Mappings.Add(new Mapping("atlasloot-enhanced", AddonSiteId.curse));
            GetAddon("AtlasLoot").Mappings.Add(new Mapping("atlasloot-enhanced", AddonSiteId.wowace));
            GetAddon("AtlasLoot").SubAddons.Add(GetAddon("AtlasLoot_BurningCrusade"));
            GetAddon("AtlasLoot").SubAddons.Add(GetAddon("AtlasLoot_Crafting"));
            GetAddon("AtlasLoot").SubAddons.Add(GetAddon("AtlasLoot_OriginalWoW"));
            GetAddon("AtlasLoot").SubAddons.Add(GetAddon("AtlasLoot_WorldEvents"));
            GetAddon("AtlasLoot").SubAddons.Add(GetAddon("AtlasLoot_WrathoftheLichKing"));
            GetAddon("AtlasLoot").SubAddons.Add(GetAddon("AtlasLootFu"));
            GetAddon("Attrition").Mappings.Add(new Mapping("10598", AddonSiteId.wowinterface));
            GetAddon("AuctionMaster").Mappings.Add(new Mapping("vendor", AddonSiteId.curse));
            GetAddon("AuctionMaster").Mappings.Add(new Mapping("vendor", AddonSiteId.curseforge));
            GetAddon("aUF_Banzai").Mappings.Add(new Mapping("a-uf_banzai", AddonSiteId.curse));
            GetAddon("aUF_Banzai").Mappings.Add(new Mapping("a-uf_banzai", AddonSiteId.wowace));
            GetAddon("AuldLangSyne").Mappings.Add(new Mapping("5320", AddonSiteId.wowinterface));
            GetAddon("AuldLangSyne").Mappings.Add(new Mapping("auld-lang-syne", AddonSiteId.curse));
            GetAddon("AuldLangSyne").Mappings.Add(new Mapping("auld-lang-syne", AddonSiteId.wowace));
            GetAddon("AutoBar").Mappings.Add(new Mapping("auto-bar", AddonSiteId.curse));
            GetAddon("AutoBar").Mappings.Add(new Mapping("auto-bar", AddonSiteId.wowace));
            GetAddon("Automaton").Mappings.Add(new Mapping("automaton", AddonSiteId.curse));
            GetAddon("Automaton").Mappings.Add(new Mapping("automaton", AddonSiteId.wowace));
            GetAddon("BadBoy").Mappings.Add(new Mapping("bad-boy", AddonSiteId.curse));
            GetAddon("BadBoy").Mappings.Add(new Mapping("bad-boy", AddonSiteId.wowace));
            GetAddon("Baggins").Mappings.Add(new Mapping("baggins", AddonSiteId.curse));
            GetAddon("Baggins").Mappings.Add(new Mapping("baggins", AddonSiteId.wowace));
            GetAddon("Baggins_ClosetGnome").Mappings.Add(new Mapping("baggins_closet-gnome", AddonSiteId.curse));
            GetAddon("Baggins_ClosetGnome").Mappings.Add(new Mapping("baggins_closet-gnome", AddonSiteId.wowace));
            GetAddon("Baggins_Outfitter").Mappings.Add(new Mapping("baggins_outfitter", AddonSiteId.curse));
            GetAddon("Baggins_Outfitter").Mappings.Add(new Mapping("baggins_outfitter", AddonSiteId.wowace));
            GetAddon("Baggins_Professions").Mappings.Add(new Mapping("baggins_professions", AddonSiteId.curse));
            GetAddon("Baggins_Professions").Mappings.Add(new Mapping("baggins_professions", AddonSiteId.wowace));
            GetAddon("Baggins_Search").Mappings.Add(new Mapping("baggins_search", AddonSiteId.curse));
            GetAddon("Baggins_Search").Mappings.Add(new Mapping("baggins_search", AddonSiteId.wowace));
            GetAddon("Baggins_SectionColor").Mappings.Add(new Mapping("baggins_section-color", AddonSiteId.curse));
            GetAddon("Baggins_SectionColor").Mappings.Add(new Mapping("baggins_section-color", AddonSiteId.wowace));
            GetAddon("Baggins_Usable").Mappings.Add(new Mapping("baggins_usable", AddonSiteId.curse));
            GetAddon("Baggins_Usable").Mappings.Add(new Mapping("baggins_usable", AddonSiteId.wowace));
            GetAddon("Bagnon").Mappings.Add(new Mapping("bagnon", AddonSiteId.curse));
            GetAddon("Bagnon").SubAddons.Add(GetAddon("Bagnon_Forever"));
            GetAddon("Bagnon").SubAddons.Add(GetAddon("Bagnon_Options"));
            GetAddon("Bagnon").SubAddons.Add(GetAddon("Bagnon_Tooltips"));
            GetAddon("Bagsy").Mappings.Add(new Mapping("10599", AddonSiteId.wowinterface));
            GetAddon("Bagsy").Mappings.Add(new Mapping("bagsy", AddonSiteId.curse));
            GetAddon("BangArtOfWar").Mappings.Add(new Mapping("11246", AddonSiteId.wowinterface));
            GetAddon("BankItems").Mappings.Add(new Mapping("bank-items", AddonSiteId.curse));
            GetAddon("BankItems").Mappings.Add(new Mapping("bank-items", AddonSiteId.wowace));
            GetAddon("BankStack").Mappings.Add(new Mapping("bank-stack", AddonSiteId.curse));
            GetAddon("BankStack").Mappings.Add(new Mapping("bank-stack", AddonSiteId.wowace));
            GetAddon("Bartender4").Mappings.Add(new Mapping("bartender4", AddonSiteId.curse));
            GetAddon("Bartender4").Mappings.Add(new Mapping("bartender4", AddonSiteId.wowace));
            GetAddon("BasicBuffs").Mappings.Add(new Mapping("basic-buffs", AddonSiteId.curse));
            GetAddon("BasicBuffs").Mappings.Add(new Mapping("basic-buffs", AddonSiteId.wowace));
            GetAddon("Bejeweled").Mappings.Add(new Mapping("1.03|24.10.2008|http://www7.popcap.com/promos/wow/|http://images.popcap.com/www/promos/wow/Bejeweled_v1_03.zip", AddonSiteId.direct));
            GetAddon("beql").Mappings.Add(new Mapping("beql", AddonSiteId.curse));
            GetAddon("beql").Mappings.Add(new Mapping("beql", AddonSiteId.wowace));
            GetAddon("BetterInbox").Mappings.Add(new Mapping("better-inbox", AddonSiteId.curse));
            GetAddon("BetterInbox").Mappings.Add(new Mapping("better-inbox", AddonSiteId.wowace));
            GetAddon("BetterQuest").Mappings.Add(new Mapping("better-quest", AddonSiteId.curse));
            GetAddon("BetterQuest").Mappings.Add(new Mapping("better-quest", AddonSiteId.wowace));
            GetAddon("BigBrother").Mappings.Add(new Mapping("big-brother", AddonSiteId.curse));
            GetAddon("BigBrother").Mappings.Add(new Mapping("big-brother", AddonSiteId.wowace));
            GetAddon("BigWigs").Mappings.Add(new Mapping("big-wigs", AddonSiteId.curse));
            GetAddon("BigWigs").Mappings.Add(new Mapping("big-wigs", AddonSiteId.wowace));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_BlackTemple"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_Extras"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_Hyjal"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_Karazhan"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_Naxxramas"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_Outland"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_Plugins"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_SC"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_Sunwell"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_TheEye"));
            GetAddon("BigWigs").SubAddons.Add(GetAddon("BigWigs_ZulAman"));
            GetAddon("BigWigs_KalecgosHealth").Mappings.Add(new Mapping("bwkh", AddonSiteId.curse));
            GetAddon("BigWigs_KalecgosHealth").Mappings.Add(new Mapping("bwkh", AddonSiteId.wowace));
            GetAddon("Blizzard_AchievementUI").Mappings.Add(new Mapping("Blizzard_AchievementUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_AuctionUI").Mappings.Add(new Mapping("Blizzard_AuctionUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_BarbershopUI").Mappings.Add(new Mapping("Blizzard_BarbershopUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_BattlefieldMinimap").Mappings.Add(new Mapping("Blizzard_BattlefieldMinimap", AddonSiteId.blizzard));
            GetAddon("Blizzard_BindingUI").Mappings.Add(new Mapping("Blizzard_BindingUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_Calendar").Mappings.Add(new Mapping("Blizzard_Calendar", AddonSiteId.blizzard));
            GetAddon("Blizzard_CombatLog").Mappings.Add(new Mapping("Blizzard_CombatLog", AddonSiteId.blizzard));
            GetAddon("Blizzard_CombatText").Mappings.Add(new Mapping("Blizzard_CombatText", AddonSiteId.blizzard));
            GetAddon("Blizzard_CraftUI").Mappings.Add(new Mapping("Blizzard_CraftUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_FeedbackUI").Mappings.Add(new Mapping("Blizzard_FeedbackUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_GlyphUI").Mappings.Add(new Mapping("Blizzard_GlyphUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_GMSurveyUI").Mappings.Add(new Mapping("Blizzard_GMSurveyUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_GuildBankUI").Mappings.Add(new Mapping("Blizzard_GuildBankUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_InspectUI").Mappings.Add(new Mapping("Blizzard_InspectUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_ItemSocketingUI").Mappings.Add(new Mapping("Blizzard_ItemSocketingUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_MacroUI").Mappings.Add(new Mapping("Blizzard_MacroUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_RaidUI").Mappings.Add(new Mapping("Blizzard_RaidUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_TalentUI").Mappings.Add(new Mapping("Blizzard_TalentUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_TimeManager").Mappings.Add(new Mapping("Blizzard_TimeManager", AddonSiteId.blizzard));
            GetAddon("Blizzard_TokenUI").Mappings.Add(new Mapping("Blizzard_TokenUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_TradeSkillUI").Mappings.Add(new Mapping("Blizzard_TradeSkillUI", AddonSiteId.blizzard));
            GetAddon("Blizzard_TrainerUI").Mappings.Add(new Mapping("Blizzard_TrainerUI", AddonSiteId.blizzard));
            GetAddon("BloodyFont").Mappings.Add(new Mapping("bloody-font-2-0", AddonSiteId.curse));
            GetAddon("BloodyFont").Mappings.Add(new Mapping("bloody-font-2-0", AddonSiteId.curseforge));
            GetAddon("Bongos").Mappings.Add(new Mapping("8419", AddonSiteId.wowinterface));
            GetAddon("Bongos").SubAddons.Add(GetAddon("Bongos_AB"));
            GetAddon("Bongos").SubAddons.Add(GetAddon("Bongos_CastBar"));
            GetAddon("Bongos").SubAddons.Add(GetAddon("Bongos_Options"));
            GetAddon("Bongos").SubAddons.Add(GetAddon("Bongos_Roll"));
            GetAddon("Bongos").SubAddons.Add(GetAddon("Bongos_Stats"));
            GetAddon("Bongos").SubAddons.Add(GetAddon("Bongos_XP"));
            GetAddon("Broker_Bags").Mappings.Add(new Mapping("broker_bags", AddonSiteId.curse));
            GetAddon("Broker_Bags").Mappings.Add(new Mapping("broker_bags", AddonSiteId.curseforge));
            GetAddon("Broker_CalendarEvents").Mappings.Add(new Mapping("broker_calendarevents", AddonSiteId.curse));
            GetAddon("Broker_CalendarEvents").Mappings.Add(new Mapping("broker_calendarevents", AddonSiteId.wowace));
            GetAddon("Broker_Clock").Mappings.Add(new Mapping("broker_clock", AddonSiteId.curse));
            GetAddon("Broker_Clock").Mappings.Add(new Mapping("broker_clock", AddonSiteId.wowace));
            GetAddon("Broker_Durability").Mappings.Add(new Mapping("broker_durability", AddonSiteId.curse));
            GetAddon("Broker_Durability").Mappings.Add(new Mapping("broker_durability", AddonSiteId.wowace));
            GetAddon("Broker_Group").Mappings.Add(new Mapping("10702", AddonSiteId.wowinterface));
            GetAddon("Broker_ItemRack").Mappings.Add(new Mapping("10610", AddonSiteId.wowinterface));
            GetAddon("Broker_Location").Mappings.Add(new Mapping("broker_location", AddonSiteId.curse));
            GetAddon("Broker_Location").Mappings.Add(new Mapping("broker_location", AddonSiteId.curseforge));
            GetAddon("Broker_Mail").Mappings.Add(new Mapping("broker_mail", AddonSiteId.curse));
            GetAddon("Broker_Mail").Mappings.Add(new Mapping("broker_mail", AddonSiteId.wowace));
            GetAddon("Broker_Money").Mappings.Add(new Mapping("broker_money", AddonSiteId.curse));
            GetAddon("Broker_Money").Mappings.Add(new Mapping("broker_money", AddonSiteId.wowace));
            GetAddon("Broker_Professions").Mappings.Add(new Mapping("broker_professions", AddonSiteId.curse));
            GetAddon("Broker_Professions").Mappings.Add(new Mapping("broker_professions", AddonSiteId.wowace));
            GetAddon("Broker_PvP").Mappings.Add(new Mapping("broker_pv-p", AddonSiteId.curse));
            GetAddon("Broker_PvP").Mappings.Add(new Mapping("broker_pv-p", AddonSiteId.wowace));
            GetAddon("Broker_Recount").Mappings.Add(new Mapping("broker_recount", AddonSiteId.curse));
            GetAddon("Broker_Recount").Mappings.Add(new Mapping("broker_recount", AddonSiteId.wowace));
            GetAddon("Broker_Regen").Mappings.Add(new Mapping("broker-regen", AddonSiteId.curse));
            GetAddon("Broker_Regen").Mappings.Add(new Mapping("broker-regen", AddonSiteId.wowace));
            GetAddon("Broker_SysMon").Mappings.Add(new Mapping("broker_sysmon", AddonSiteId.curse));
            GetAddon("Broker_SysMon").Mappings.Add(new Mapping("broker_sysmon", AddonSiteId.wowace));
            GetAddon("Broker_Tracking").Mappings.Add(new Mapping("broker_tracking", AddonSiteId.curse));
            GetAddon("Broker_Tracking").Mappings.Add(new Mapping("broker_tracking", AddonSiteId.wowace));
            GetAddon("Broker2FuBar").Mappings.Add(new Mapping("broker2fubar", AddonSiteId.curse));
            GetAddon("Broker2FuBar").Mappings.Add(new Mapping("broker2fubar", AddonSiteId.wowace));
            GetAddon("Buffalo").Mappings.Add(new Mapping("buffalo", AddonSiteId.curse));
            GetAddon("Buffalo").Mappings.Add(new Mapping("buffalo", AddonSiteId.wowace));
            GetAddon("BuffEnough").Mappings.Add(new Mapping("buff-enough", AddonSiteId.curse));
            GetAddon("BuffEnough").Mappings.Add(new Mapping("buff-enough", AddonSiteId.wowace));
            GetAddon("buffet").Mappings.Add(new Mapping("8370", AddonSiteId.wowinterface));
            GetAddon("buffet").Mappings.Add(new Mapping("buffet", AddonSiteId.curse));
            GetAddon("buffet").Mappings.Add(new Mapping("buffet", AddonSiteId.wowace));
            GetAddon("BugSack").Mappings.Add(new Mapping("bugsack", AddonSiteId.curse));
            GetAddon("BugSack").Mappings.Add(new Mapping("bugsack", AddonSiteId.wowace));
            GetAddon("Butsu").Mappings.Add(new Mapping("7976", AddonSiteId.wowinterface));
            GetAddon("ButtonBin").Mappings.Add(new Mapping("button-bin", AddonSiteId.curse));
            GetAddon("ButtonBin").Mappings.Add(new Mapping("button-bin", AddonSiteId.wowace));
            GetAddon("ButtonFacade").Mappings.Add(new Mapping("buttonfacade", AddonSiteId.curse));
            GetAddon("ButtonFacade").Mappings.Add(new Mapping("buttonfacade", AddonSiteId.wowace));
            GetAddon("ButtonFacade_Apathy").Mappings.Add(new Mapping("buttonfacade_apathy", AddonSiteId.curse));
            GetAddon("ButtonFacade_Apathy").Mappings.Add(new Mapping("buttonfacade_apathy", AddonSiteId.wowace));
            GetAddon("ButtonFacade_Caith").Mappings.Add(new Mapping("buttonfacade_caith", AddonSiteId.curse));
            GetAddon("ButtonFacade_Caith").Mappings.Add(new Mapping("buttonfacade_caith", AddonSiteId.wowace));
            GetAddon("ButtonFacade_DsmFade").Mappings.Add(new Mapping("11363", AddonSiteId.wowinterface));
            GetAddon("ButtonFacade_Elegance").Mappings.Add(new Mapping("9623", AddonSiteId.wowinterface));
            GetAddon("ButtonFacade_Entropy").Mappings.Add(new Mapping("buttonfacade_entropy", AddonSiteId.curse));
            GetAddon("ButtonFacade_Entropy").Mappings.Add(new Mapping("buttonfacade_entropy", AddonSiteId.wowace));
            GetAddon("ButtonFacade_Gears").Mappings.Add(new Mapping("buttonfacade_gears", AddonSiteId.curse));
            GetAddon("ButtonFacade_Gears").Mappings.Add(new Mapping("buttonfacade_gears", AddonSiteId.wowace));
            GetAddon("ButtonFacade_HKitty").Mappings.Add(new Mapping("button-facade_hkitty", AddonSiteId.curse));
            GetAddon("ButtonFacade_HKitty").Mappings.Add(new Mapping("button-facade_hkitty", AddonSiteId.wowace));
            GetAddon("ButtonFacade_ItemRack").Mappings.Add(new Mapping("bfir", AddonSiteId.curse));
            GetAddon("ButtonFacade_LayerTest").Mappings.Add(new Mapping("buttonfacade_layertest", AddonSiteId.curse));
            GetAddon("ButtonFacade_LayerTest").Mappings.Add(new Mapping("buttonfacade_layertest", AddonSiteId.wowace));
            GetAddon("ButtonFacade_LiteStep").Mappings.Add(new Mapping("buttonfacade_litestep", AddonSiteId.curse));
            GetAddon("ButtonFacade_LiteStep").Mappings.Add(new Mapping("buttonfacade_litestep", AddonSiteId.wowace));
            GetAddon("ButtonFacade_Onyx").Mappings.Add(new Mapping("buttonfacade_onyx", AddonSiteId.curse));
            GetAddon("ButtonFacade_Onyx").Mappings.Add(new Mapping("buttonfacade_onyx", AddonSiteId.wowace));
            GetAddon("ButtonFacade_Serenity").Mappings.Add(new Mapping("buttonfacade_serenity", AddonSiteId.curse));
            GetAddon("ButtonFacade_Serenity").Mappings.Add(new Mapping("buttonfacade_serenity", AddonSiteId.wowace));
            GetAddon("ButtonFacade_simpleSquare").Mappings.Add(new Mapping("button-facade_simple-square", AddonSiteId.curse));
            GetAddon("ButtonFacade_simpleSquare").Mappings.Add(new Mapping("button-facade_simple-square", AddonSiteId.wowace));
            GetAddon("ButtonFacade_Sleek").Mappings.Add(new Mapping("11010", AddonSiteId.wowinterface));
            GetAddon("ButtonFacade_Tones").Mappings.Add(new Mapping("9712", AddonSiteId.wowinterface));
            GetAddon("ButtonFacade_Trinity").Mappings.Add(new Mapping("button-facade-trinity", AddonSiteId.curse));
            GetAddon("CandyBar").Mappings.Add(new Mapping("candybar", AddonSiteId.curse));
            GetAddon("CandyBar").Mappings.Add(new Mapping("candybar", AddonSiteId.wowace));
            GetAddon("Capping").Mappings.Add(new Mapping("11177", AddonSiteId.wowinterface));
            GetAddon("Capping").Mappings.Add(new Mapping("capping", AddonSiteId.curse));
            GetAddon("Capping").Mappings.Add(new Mapping("capping", AddonSiteId.wowace));
            GetAddon("Carbonite").Mappings.Add(new Mapping("carbonite-quest", AddonSiteId.curse));
            GetAddon("cargoHonor").Mappings.Add(new Mapping("10482", AddonSiteId.wowinterface));
            GetAddon("Cartographer").Mappings.Add(new Mapping("cartographer", AddonSiteId.curse));
            GetAddon("Cartographer").Mappings.Add(new Mapping("cartographer", AddonSiteId.wowace));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_Battlegrounds"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_Coordinates"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_Foglight"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_GroupColors"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_GuildPositions"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_InstanceLoot"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_InstanceMaps"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_InstanceNotes"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_LookNFeel"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_Notes"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_POI"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_Professions"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_Waypoints"));
            GetAddon("Cartographer").SubAddons.Add(GetAddon("Cartographer_ZoneInfo"));
            GetAddon("Cartographer3").Mappings.Add(new Mapping("cartographer3", AddonSiteId.curse));
            GetAddon("Cartographer3").Mappings.Add(new Mapping("cartographer3", AddonSiteId.wowace));
            GetAddon("Cartographer3").SubAddons.Add(GetAddon("Cartographer3_InstancePOIs"));
            GetAddon("Cartographer3").SubAddons.Add(GetAddon("Cartographer3_Notes"));
            GetAddon("Cartographer3").SubAddons.Add(GetAddon("Cartographer3_Waypoints"));
            GetAddon("CastYeller").Mappings.Add(new Mapping("cast-yeller", AddonSiteId.curse));
            GetAddon("CastYeller").Mappings.Add(new Mapping("cast-yeller", AddonSiteId.wowace));
            GetAddon("CCBreaker").Mappings.Add(new Mapping("ccbreaker", AddonSiteId.curse));
            GetAddon("CCBreaker").Mappings.Add(new Mapping("ccbreaker", AddonSiteId.wowace));
            GetAddon("CCTracker").Mappings.Add(new Mapping("9051", AddonSiteId.wowinterface));
            GetAddon("Chaman2").Mappings.Add(new Mapping("chaman2", AddonSiteId.curse));
            GetAddon("Chaman2").Mappings.Add(new Mapping("chaman2", AddonSiteId.wowace));
            GetAddon("Chaman2").Mappings.Add(new Mapping("totem-manager", AddonSiteId.curse));
            GetAddon("Chaman2").Mappings.Add(new Mapping("totem-manager", AddonSiteId.curseforge));
            GetAddon("ChatBar").Mappings.Add(new Mapping("4422", AddonSiteId.wowinterface));
            GetAddon("Chatter").Mappings.Add(new Mapping("chatter", AddonSiteId.curse));
            GetAddon("Chatter").Mappings.Add(new Mapping("chatter", AddonSiteId.wowace));
            GetAddon("ChatThrottleLib").Mappings.Add(new Mapping("chatthrottlelib", AddonSiteId.curse));
            GetAddon("ChatThrottleLib").Mappings.Add(new Mapping("chatthrottlelib", AddonSiteId.wowace));
            GetAddon("Chinchilla").Mappings.Add(new Mapping("chinchilla", AddonSiteId.curse));
            GetAddon("Chinchilla").Mappings.Add(new Mapping("chinchilla", AddonSiteId.wowace));
            GetAddon("ClassLoot").Mappings.Add(new Mapping("classloot", AddonSiteId.curse));
            GetAddon("ClassLoot").Mappings.Add(new Mapping("classloot", AddonSiteId.wowace));
            GetAddon("ClassTimer").Mappings.Add(new Mapping("classtimer", AddonSiteId.curse));
            GetAddon("ClassTimer").Mappings.Add(new Mapping("classtimer", AddonSiteId.wowace));
            GetAddon("ClearFont2").Mappings.Add(new Mapping("clear-font2", AddonSiteId.curse));
            GetAddon("ClearFont2").Mappings.Add(new Mapping("clear-font2", AddonSiteId.wowace));
            GetAddon("ClearFont2_FontPack").Mappings.Add(new Mapping("clear-font2_font-pack", AddonSiteId.curse));
            GetAddon("ClearFont2_FontPack").Mappings.Add(new Mapping("clear-font2_font-pack", AddonSiteId.wowace));
            GetAddon("Click2Cast").Mappings.Add(new Mapping("click2cast", AddonSiteId.curse));
            GetAddon("Click2Cast").Mappings.Add(new Mapping("click2cast", AddonSiteId.wowace));
            GetAddon("Clique").Mappings.Add(new Mapping("5108", AddonSiteId.wowinterface));
            GetAddon("ClosetGnome").Mappings.Add(new Mapping("closet-gnome", AddonSiteId.curse));
            GetAddon("ClosetGnome").Mappings.Add(new Mapping("closet-gnome", AddonSiteId.wowace));
            GetAddon("ClosetGnome_Banker").Mappings.Add(new Mapping("closet-gnome_banker", AddonSiteId.curse));
            GetAddon("ClosetGnome_Banker").Mappings.Add(new Mapping("closet-gnome_banker", AddonSiteId.wowace));
            GetAddon("ClosetGnome_HelmNCloak").Mappings.Add(new Mapping("closet-gnome_helm-ncloak", AddonSiteId.curse));
            GetAddon("ClosetGnome_HelmNCloak").Mappings.Add(new Mapping("closet-gnome_helm-ncloak", AddonSiteId.wowace));
            GetAddon("ClosetGnome_Mount").Mappings.Add(new Mapping("closet-gnome_mount", AddonSiteId.curse));
            GetAddon("ClosetGnome_Mount").Mappings.Add(new Mapping("closet-gnome_mount", AddonSiteId.wowace));
            GetAddon("ClosetGnome_Tooltip").Mappings.Add(new Mapping("closet-gnome_tooltip", AddonSiteId.curse));
            GetAddon("ClosetGnome_Tooltip").Mappings.Add(new Mapping("closet-gnome_tooltip", AddonSiteId.wowace));
            GetAddon("Coconuts").Mappings.Add(new Mapping("coconuts", AddonSiteId.curse));
            GetAddon("Coconuts").Mappings.Add(new Mapping("coconuts", AddonSiteId.wowace));
            GetAddon("Combuctor").Mappings.Add(new Mapping("8113", AddonSiteId.wowinterface));
            GetAddon("Combuctor").Mappings.Add(new Mapping("combuctor", AddonSiteId.curse));
            GetAddon("Combuctor").Mappings.Add(new Mapping("combuctor", AddonSiteId.curseforge));
            GetAddon("Combuctor").SubAddons.Add(GetAddon("Bagnon_Forever"));
            GetAddon("Combuctor").SubAddons.Add(GetAddon("Bagnon_Tooltips"));
            GetAddon("Combuctor").SubAddons.Add(GetAddon("Combuctor_Config"));
            GetAddon("Combuctor").SubAddons.Add(GetAddon("Combuctor_Sets"));
            GetAddon("Comix").Mappings.Add(new Mapping("comix-the-return", AddonSiteId.curse));
            GetAddon("CooldownCount").Mappings.Add(new Mapping("cooldown-count", AddonSiteId.curse));
            GetAddon("CooldownCount").Mappings.Add(new Mapping("cooldown-count", AddonSiteId.wowace));
            GetAddon("CooldownTimers2").Mappings.Add(new Mapping("cooldown-timers2", AddonSiteId.curse));
            GetAddon("CooldownTimers2").Mappings.Add(new Mapping("cooldown-timers2", AddonSiteId.wowace));
            GetAddon("CowTip").Mappings.Add(new Mapping("cowtip", AddonSiteId.curse));
            GetAddon("CowTip").Mappings.Add(new Mapping("cowtip", AddonSiteId.wowace));
            GetAddon("CreatureComforts").Mappings.Add(new Mapping("9635", AddonSiteId.wowinterface));
            GetAddon("CreatureComforts").Mappings.Add(new Mapping("creaturecomforts", AddonSiteId.curse));
            GetAddon("Cryolysis2").Mappings.Add(new Mapping("cryolysis2", AddonSiteId.curse));
            GetAddon("Cryolysis2").Mappings.Add(new Mapping("cryolysis2", AddonSiteId.wowace));
            GetAddon("CT_Core").Mappings.Add(new Mapping("CT_Core", AddonSiteId.wowspecial));
            GetAddon("CT_RaidTracker").Mappings.Add(new Mapping("CT_RaidTracker", AddonSiteId.wowspecial));
            GetAddon("CT_UnitFrames").Mappings.Add(new Mapping("CT_UnitFrames", AddonSiteId.wowspecial));
            GetAddon("CursorCastbar").Mappings.Add(new Mapping("11067", AddonSiteId.wowinterface));
            GetAddon("DagAssist").Mappings.Add(new Mapping("11358", AddonSiteId.wowinterface));
            GetAddon("Damnation").Mappings.Add(new Mapping("damnation", AddonSiteId.curse));
            GetAddon("Damnation").Mappings.Add(new Mapping("damnation", AddonSiteId.wowace));
            GetAddon("DBM_API").Mappings.Add(new Mapping("deadly-boss-mods", AddonSiteId.curse));
            GetAddon("DBM_API").Mappings.Add(new Mapping("deadly-boss-mods", AddonSiteId.wowace));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_Battlegrounds"));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_BlackTemple"));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_GUI"));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_Hyjal"));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_Karazhan"));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_Outlands"));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_Serpentshrine"));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_Sunwell"));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_TheEye"));
            GetAddon("DBM_API").SubAddons.Add(GetAddon("DBM_ZulAman"));
            GetAddon("Decursive").Mappings.Add(new Mapping("decursive", AddonSiteId.curse));
            GetAddon("Decursive").Mappings.Add(new Mapping("decursive", AddonSiteId.curseforge));
            GetAddon("Dock").Mappings.Add(new Mapping("dock", AddonSiteId.curse));
            GetAddon("Dock").Mappings.Add(new Mapping("dock", AddonSiteId.wowace));
            GetAddon("Dominos").Mappings.Add(new Mapping("9085", AddonSiteId.wowinterface));
            GetAddon("Dominos").Mappings.Add(new Mapping("dominos", AddonSiteId.curse));
            GetAddon("Dominos").SubAddons.Add(GetAddon("Dominos_Buff"));
            GetAddon("Dominos").SubAddons.Add(GetAddon("Dominos_Cast"));
            GetAddon("Dominos").SubAddons.Add(GetAddon("Dominos_Config"));
            GetAddon("Dominos").SubAddons.Add(GetAddon("Dominos_Roll"));
            GetAddon("Dominos").SubAddons.Add(GetAddon("Dominos_XP"));
            GetAddon("DotDotDot").Mappings.Add(new Mapping("dot-dot-dot", AddonSiteId.curse));
            GetAddon("DotDotDot").Mappings.Add(new Mapping("dot-dot-dot", AddonSiteId.wowace));
            GetAddon("DoTimer").Mappings.Add(new Mapping("do-timer", AddonSiteId.curse));
            GetAddon("DoTimer").SubAddons.Add(GetAddon("DoTimer_Options"));
            GetAddon("DPS").Mappings.Add(new Mapping("11059", AddonSiteId.wowinterface));
            GetAddon("DrDamage").Mappings.Add(new Mapping("dr-damage", AddonSiteId.curse));
            GetAddon("DrDamage").Mappings.Add(new Mapping("dr-damage", AddonSiteId.wowace));
            GetAddon("DRTracker").Mappings.Add(new Mapping("8901", AddonSiteId.wowinterface));
            GetAddon("DruidBar").Mappings.Add(new Mapping("druid-bar", AddonSiteId.curse));
            GetAddon("DynamicPerformance").Mappings.Add(new Mapping("dynamic-performance", AddonSiteId.curse));
            GetAddon("EasyMother").Mappings.Add(new Mapping("easy-mother", AddonSiteId.curse));
            GetAddon("EavesDrop").Mappings.Add(new Mapping("eaves-drop", AddonSiteId.curse));
            GetAddon("EavesDrop").Mappings.Add(new Mapping("eaves-drop", AddonSiteId.wowace));
            GetAddon("eCastingBar").Mappings.Add(new Mapping("e-casting-bar-for-wo-w-2-0", AddonSiteId.curse));
            GetAddon("eePanels2").Mappings.Add(new Mapping("ee-panels2", AddonSiteId.curse));
            GetAddon("eePanels2").Mappings.Add(new Mapping("ee-panels2", AddonSiteId.wowace));
            GetAddon("ElkBuffBars").Mappings.Add(new Mapping("elkbuffbars", AddonSiteId.curse));
            GetAddon("ElkBuffBars").Mappings.Add(new Mapping("elkbuffbars", AddonSiteId.wowace));
            GetAddon("Engravings").Mappings.Add(new Mapping("4858", AddonSiteId.wowinterface));
            GetAddon("epgp").Mappings.Add(new Mapping("epgp-dkp-reloaded", AddonSiteId.curse));
            GetAddon("EQCompare").Mappings.Add(new Mapping("eqcompare", AddonSiteId.curse));
            GetAddon("EQCompare").Mappings.Add(new Mapping("eqcompare", AddonSiteId.wowace));
            GetAddon("EquipCompare").Mappings.Add(new Mapping("4392", AddonSiteId.wowinterface));
            GetAddon("ErrorMonster").Mappings.Add(new Mapping("error-monster", AddonSiteId.curse));
            GetAddon("ErrorMonster").Mappings.Add(new Mapping("error-monster", AddonSiteId.wowace));
            GetAddon("EveryQuest").Mappings.Add(new Mapping("everyquest", AddonSiteId.curse));
            GetAddon("EveryQuest").Mappings.Add(new Mapping("everyquest", AddonSiteId.wowace));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Battlegrounds"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Classes"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Dungeons"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Eastern_Kingdoms"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Kalimdor"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Miscellaneous"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Northrend"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Outland"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Professions"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Raids"));
            GetAddon("EveryQuest").SubAddons.Add(GetAddon("EveryQuest_Seasonal"));
            GetAddon("Examiner").Mappings.Add(new Mapping("7377", AddonSiteId.wowinterface));
            GetAddon("Examiner").Mappings.Add(new Mapping("examiner", AddonSiteId.curse));
            GetAddon("ExpressMail").Mappings.Add(new Mapping("6439", AddonSiteId.wowinterface));
            GetAddon("Fane").Mappings.Add(new Mapping("7984", AddonSiteId.wowinterface));
            GetAddon("FeedMachine").Mappings.Add(new Mapping("feed-machine", AddonSiteId.curse));
            GetAddon("Fence").Mappings.Add(new Mapping("fence", AddonSiteId.curse));
            GetAddon("Fence").Mappings.Add(new Mapping("fence", AddonSiteId.wowace));
            GetAddon("FishermansFriend").Mappings.Add(new Mapping("fishermans-friend", AddonSiteId.curse));
            GetAddon("FishermansFriend").Mappings.Add(new Mapping("fishermans-friend", AddonSiteId.wowace));
            GetAddon("Fizzle").Mappings.Add(new Mapping("5018", AddonSiteId.wowinterface));
            GetAddon("FlightMap").Mappings.Add(new Mapping("flight-map", AddonSiteId.curse));
            GetAddon("FloatingFrames").Mappings.Add(new Mapping("fframes", AddonSiteId.curse));
            GetAddon("FocusFrame").Mappings.Add(new Mapping("focus-frame", AddonSiteId.curse));
            GetAddon("fontain").Mappings.Add(new Mapping("fontain", AddonSiteId.curse));
            GetAddon("fontain").Mappings.Add(new Mapping("fontain", AddonSiteId.wowace));
            GetAddon("Fortress").Mappings.Add(new Mapping("fortress", AddonSiteId.curse));
            GetAddon("Fortress").Mappings.Add(new Mapping("fortress", AddonSiteId.wowace));
            GetAddon("FreeRefills").Mappings.Add(new Mapping("free-refills", AddonSiteId.curse));
            GetAddon("FreeRefills").Mappings.Add(new Mapping("free-refills", AddonSiteId.wowace));
            GetAddon("FuBar").Mappings.Add(new Mapping("fubar", AddonSiteId.curse));
            GetAddon("FuBar").Mappings.Add(new Mapping("fubar", AddonSiteId.wowace));
            GetAddon("FuBar_AlchemyFu").Mappings.Add(new Mapping("fubar_alchemyfu", AddonSiteId.curse));
            GetAddon("FuBar_AlchemyFu").Mappings.Add(new Mapping("fubar_alchemyfu", AddonSiteId.wowace));
            GetAddon("FuBar_AmmoFu").Mappings.Add(new Mapping("fubar_ammofu", AddonSiteId.curse));
            GetAddon("FuBar_AmmoFu").Mappings.Add(new Mapping("fubar_ammofu", AddonSiteId.wowace));
            GetAddon("FuBar_AuditorFu").Mappings.Add(new Mapping("5429", AddonSiteId.wowinterface));
            GetAddon("FuBar_BagFu").Mappings.Add(new Mapping("fu-bar_bag-fu", AddonSiteId.curse));
            GetAddon("FuBar_BagFu").Mappings.Add(new Mapping("fu-bar_bag-fu", AddonSiteId.wowace));
            GetAddon("FuBar_ClockFu").Mappings.Add(new Mapping("fubar_clockfu", AddonSiteId.curse));
            GetAddon("FuBar_ClockFu").Mappings.Add(new Mapping("fubar_clockfu", AddonSiteId.wowace));
            GetAddon("FuBar_DurabilityFu").Mappings.Add(new Mapping("fubar_durabilityfu", AddonSiteId.curse));
            GetAddon("FuBar_DurabilityFu").Mappings.Add(new Mapping("fubar_durabilityfu", AddonSiteId.wowace));
            GetAddon("FuBar_DuraTek").Mappings.Add(new Mapping("fu-bar_dura-tek", AddonSiteId.curse));
            GetAddon("FuBar_DuraTek").Mappings.Add(new Mapping("fu-bar_dura-tek", AddonSiteId.wowace));
            GetAddon("FuBar_ExperienceFu").Mappings.Add(new Mapping("fubar_experiencefu", AddonSiteId.curse));
            GetAddon("FuBar_ExperienceFu").Mappings.Add(new Mapping("fubar_experiencefu", AddonSiteId.wowace));
            GetAddon("FuBar_FactionsFu").Mappings.Add(new Mapping("fubar_factionsfu", AddonSiteId.curse));
            GetAddon("FuBar_FactionsFu").Mappings.Add(new Mapping("fubar_factionsfu", AddonSiteId.wowace));
            GetAddon("FuBar_FriendsFu").Mappings.Add(new Mapping("fubar_friendsfu", AddonSiteId.curse));
            GetAddon("FuBar_FriendsFu").Mappings.Add(new Mapping("fubar_friendsfu", AddonSiteId.wowace));
            GetAddon("FuBar_FuXPFu").Mappings.Add(new Mapping("fu-bar_fu-xpfu", AddonSiteId.curse));
            GetAddon("FuBar_FuXPFu").Mappings.Add(new Mapping("fu-bar_fu-xpfu", AddonSiteId.wowace));
            GetAddon("FuBar_GarbageFu").Mappings.Add(new Mapping("fu-bar_garbage-fu", AddonSiteId.curse));
            GetAddon("FuBar_GarbageFu").Mappings.Add(new Mapping("fu-bar_garbage-fu", AddonSiteId.wowace));
            GetAddon("FuBar_GroupFu").Mappings.Add(new Mapping("fubar_groupfu", AddonSiteId.curse));
            GetAddon("FuBar_GroupFu").Mappings.Add(new Mapping("fubar_groupfu", AddonSiteId.wowace));
            GetAddon("FuBar_GuildFu").Mappings.Add(new Mapping("fubar_guildfu", AddonSiteId.curse));
            GetAddon("FuBar_GuildFu").Mappings.Add(new Mapping("fubar_guildfu", AddonSiteId.wowace));
            GetAddon("FuBar_HealBotFu").Mappings.Add(new Mapping("fu-bar_heal-bot-fu", AddonSiteId.curse));
            GetAddon("FuBar_HealBotFu").Mappings.Add(new Mapping("fu-bar_heal-bot-fu", AddonSiteId.wowace));
            GetAddon("FuBar_HonorFu").Mappings.Add(new Mapping("fubar_honorfu", AddonSiteId.curse));
            GetAddon("FuBar_HonorFu").Mappings.Add(new Mapping("fubar_honorfu", AddonSiteId.wowace));
            GetAddon("FuBar_ItemBonusesFu").Mappings.Add(new Mapping("fu-bar_item-bonuses-fu", AddonSiteId.curse));
            GetAddon("FuBar_ItemBonusesFu").Mappings.Add(new Mapping("fu-bar_item-bonuses-fu", AddonSiteId.wowace));
            GetAddon("FuBar_ItemRackFu").Mappings.Add(new Mapping("itemrackfu", AddonSiteId.curse));
            GetAddon("FuBar_ItemRackFu").Mappings.Add(new Mapping("itemrackfu", AddonSiteId.wowace));
            GetAddon("FuBar_LastPlayedFu").Mappings.Add(new Mapping("lastplayedfu", AddonSiteId.curse));
            GetAddon("FuBar_LastPlayedFu").Mappings.Add(new Mapping("lastplayedfu", AddonSiteId.curseforge));
            GetAddon("FuBar_LocationFu").Mappings.Add(new Mapping("fubar_locationfu", AddonSiteId.curse));
            GetAddon("FuBar_LocationFu").Mappings.Add(new Mapping("fubar_locationfu", AddonSiteId.wowace));
            GetAddon("Fubar_MacroFu").Mappings.Add(new Mapping("5039", AddonSiteId.wowinterface));
            GetAddon("FuBar_MailExpiryFu").Mappings.Add(new Mapping("mailexpiryfu", AddonSiteId.curse));
            GetAddon("FuBar_MailExpiryFu").Mappings.Add(new Mapping("mailexpiryfu", AddonSiteId.curseforge));
            GetAddon("FuBar_MicroMenuFu").Mappings.Add(new Mapping("fubar_micromenufu", AddonSiteId.curse));
            GetAddon("FuBar_MicroMenuFu").Mappings.Add(new Mapping("fubar_micromenufu", AddonSiteId.wowace));
            GetAddon("FuBar_MoneyFu").Mappings.Add(new Mapping("fu-bar_money-fu", AddonSiteId.curse));
            GetAddon("FuBar_MoneyFu").Mappings.Add(new Mapping("fu-bar_money-fu", AddonSiteId.wowace));
            GetAddon("FuBar_NameToggleFu").Mappings.Add(new Mapping("fubar_nametogglefu", AddonSiteId.curse));
            GetAddon("FuBar_NameToggleFu").Mappings.Add(new Mapping("fubar_nametogglefu", AddonSiteId.wowace));
            GetAddon("FuBar_PerformanceFu").Mappings.Add(new Mapping("fubar_performancefu", AddonSiteId.curse));
            GetAddon("FuBar_PerformanceFu").Mappings.Add(new Mapping("fubar_performancefu", AddonSiteId.wowace));
            GetAddon("FuBar_PetFu").Mappings.Add(new Mapping("fu-bar_pet-fu", AddonSiteId.curse));
            GetAddon("FuBar_PetFu").Mappings.Add(new Mapping("fu-bar_pet-fu", AddonSiteId.wowace));
            GetAddon("FuBar_QuestsFu").Mappings.Add(new Mapping("fu-bar_quests-fu", AddonSiteId.curse));
            GetAddon("FuBar_QuestsFu").Mappings.Add(new Mapping("fu-bar_quests-fu", AddonSiteId.wowace));
            GetAddon("FuBar_RaidBuffFu").Mappings.Add(new Mapping("fu-bar_raid-buff-fu", AddonSiteId.curse));
            GetAddon("FuBar_RaidBuffFu").Mappings.Add(new Mapping("fu-bar_raid-buff-fu", AddonSiteId.wowace));
            GetAddon("FuBar_ReagentFu").Mappings.Add(new Mapping("fu-bar_reagent-fu", AddonSiteId.curse));
            GetAddon("FuBar_ReagentFu").Mappings.Add(new Mapping("fu-bar_reagent-fu", AddonSiteId.wowace));
            GetAddon("FuBar_RecountFu").Mappings.Add(new Mapping("fu-bar_recount-fu", AddonSiteId.curse));
            GetAddon("FuBar_RecountFu").Mappings.Add(new Mapping("fu-bar_recount-fu", AddonSiteId.wowace));
            GetAddon("FuBar_SkillsPlusFu").Mappings.Add(new Mapping("fu-bar_skills-plus-fu", AddonSiteId.curse));
            GetAddon("FuBar_SkillsPlusFu").Mappings.Add(new Mapping("fu-bar_skills-plus-fu", AddonSiteId.wowace));
            GetAddon("FuBar_SpeedFu").Mappings.Add(new Mapping("fu-bar_speed-fu", AddonSiteId.curse));
            GetAddon("FuBar_SpeedFu").Mappings.Add(new Mapping("fu-bar_speed-fu", AddonSiteId.wowace));
            GetAddon("FuBar_ToFu").Mappings.Add(new Mapping("fu-bar_to-fu", AddonSiteId.curse));
            GetAddon("FuBar_ToFu").Mappings.Add(new Mapping("fu-bar_to-fu", AddonSiteId.wowace));
            GetAddon("FuBar_TopScoreFu").Mappings.Add(new Mapping("fu-bar_top-score-fu", AddonSiteId.curse));
            GetAddon("FuBar_TopScoreFu").Mappings.Add(new Mapping("fu-bar_top-score-fu", AddonSiteId.wowace));
            GetAddon("FuBar_TrainerFu").Mappings.Add(new Mapping("fu-bar_trainer-fu", AddonSiteId.curse));
            GetAddon("FuBar_TrainerFu").Mappings.Add(new Mapping("fu-bar_trainer-fu", AddonSiteId.wowace));
            GetAddon("FuBar_TransporterFu").Mappings.Add(new Mapping("fu-bar_transporter-fu", AddonSiteId.curse));
            GetAddon("FuBar_TransporterFu").Mappings.Add(new Mapping("fu-bar_transporter-fu", AddonSiteId.wowace));
            GetAddon("FuBar_VolumeFu").Mappings.Add(new Mapping("fu-bar_volume-fu", AddonSiteId.curse));
            GetAddon("FuBar_VolumeFu").Mappings.Add(new Mapping("fu-bar_volume-fu", AddonSiteId.wowace));
            GetAddon("FuBar2Broker").Mappings.Add(new Mapping("fubar2broker", AddonSiteId.curse));
            GetAddon("FuBar2Broker").Mappings.Add(new Mapping("fubar2broker", AddonSiteId.wowace));
            GetAddon("FuBarPlugin-2.0").Mappings.Add(new Mapping("fubarplugin-2-0", AddonSiteId.curse));
            GetAddon("FuBarPlugin-2.0").Mappings.Add(new Mapping("fubarplugin-2-0", AddonSiteId.wowace));
            GetAddon("FuTextures").Mappings.Add(new Mapping("futextures", AddonSiteId.curse));
            GetAddon("FuTextures").Mappings.Add(new Mapping("futextures", AddonSiteId.wowace));
            GetAddon("GatherHud").Mappings.Add(new Mapping("gather-hud", AddonSiteId.curse));
            GetAddon("GatherHud").Mappings.Add(new Mapping("gather-hud", AddonSiteId.wowace));
            GetAddon("GatherMate").Mappings.Add(new Mapping("gathermate", AddonSiteId.curse));
            GetAddon("GatherMate").Mappings.Add(new Mapping("gathermate", AddonSiteId.wowace));
            GetAddon("GatherMate_Data").Mappings.Add(new Mapping("gathermate_data", AddonSiteId.curse));
            GetAddon("GatherMate_Data").Mappings.Add(new Mapping("gathermate_data", AddonSiteId.wowace));
            GetAddon("GatherMate_Sharing").Mappings.Add(new Mapping("gathermate_sharing", AddonSiteId.curse));
            GetAddon("GatherMate_Sharing").Mappings.Add(new Mapping("gathermate_sharing", AddonSiteId.wowace));
            GetAddon("GFW_FactionFriend").Mappings.Add(new Mapping("6402", AddonSiteId.wowinterface));
            GetAddon("GFW_FeedOMatic").Mappings.Add(new Mapping("4160", AddonSiteId.wowinterface));
            GetAddon("GhostPulse").Mappings.Add(new Mapping("8418", AddonSiteId.wowinterface));
            GetAddon("GloryLib").Mappings.Add(new Mapping("glorylib", AddonSiteId.curse));
            GetAddon("GloryLib").Mappings.Add(new Mapping("glorylib", AddonSiteId.wowace));
            GetAddon("GoGoMount").Mappings.Add(new Mapping("go-go-mount", AddonSiteId.curse));
            GetAddon("Grid").Mappings.Add(new Mapping("grid", AddonSiteId.curse));
            GetAddon("Grid").Mappings.Add(new Mapping("grid", AddonSiteId.wowace));
            GetAddon("Grid2").Mappings.Add(new Mapping("grid2", AddonSiteId.curse));
            GetAddon("Grid2").Mappings.Add(new Mapping("grid2", AddonSiteId.wowace));
            GetAddon("Grid2").SubAddons.Add(GetAddon("Grid2Alert"));
            GetAddon("Grid2").SubAddons.Add(GetAddon("Grid2Options"));
            GetAddon("Grid2").SubAddons.Add(GetAddon("Grid2StatusRaidDebuffs"));
            GetAddon("GridIndicatorSideIcons").Mappings.Add(new Mapping("grid-indicator-side-icons", AddonSiteId.curse));
            GetAddon("GridIndicatorSideIcons").Mappings.Add(new Mapping("grid-indicator-side-icons", AddonSiteId.wowace));
            GetAddon("GridIndicatorText3").Mappings.Add(new Mapping("grid-indicator-text3", AddonSiteId.curse));
            GetAddon("GridIndicatorText3").Mappings.Add(new Mapping("grid-indicator-text3", AddonSiteId.wowace));
            GetAddon("GridLayoutPlus").Mappings.Add(new Mapping("grid-layout-plus", AddonSiteId.curse));
            GetAddon("GridLayoutPlus").Mappings.Add(new Mapping("grid-layout-plus", AddonSiteId.wowace));
            GetAddon("GridManaBars").Mappings.Add(new Mapping("grid-mana-bars", AddonSiteId.curse));
            GetAddon("GridManaBars").Mappings.Add(new Mapping("grid-mana-bars", AddonSiteId.wowace));
            GetAddon("GridSideIndicators").Mappings.Add(new Mapping("grid-side-indicators", AddonSiteId.curse));
            GetAddon("GridSideIndicators").Mappings.Add(new Mapping("grid-side-indicators", AddonSiteId.wowace));
            GetAddon("GridStatusAFK").Mappings.Add(new Mapping("grid-status-afk", AddonSiteId.curse));
            GetAddon("GridStatusAFK").Mappings.Add(new Mapping("grid-status-afk", AddonSiteId.wowace));
            GetAddon("GridStatusHots").Mappings.Add(new Mapping("grid-status-hots", AddonSiteId.curse));
            GetAddon("GridStatusHots").Mappings.Add(new Mapping("grid-status-hots", AddonSiteId.wowace));
            GetAddon("GridStatusHotStack").Mappings.Add(new Mapping("grid-status-hot-stack", AddonSiteId.curse));
            GetAddon("GridStatusHotStack").Mappings.Add(new Mapping("grid-status-hot-stack", AddonSiteId.wowace));
            GetAddon("GridStatusIncomingHeals").Mappings.Add(new Mapping("grid-status-incoming-heals", AddonSiteId.curse));
            GetAddon("GridStatusIncomingHeals").Mappings.Add(new Mapping("grid-status-incoming-heals", AddonSiteId.wowace));
            GetAddon("GridStatusLifebloom").Mappings.Add(new Mapping("grid-status-lifebloom", AddonSiteId.curse));
            GetAddon("GridStatusLifebloom").Mappings.Add(new Mapping("grid-status-lifebloom", AddonSiteId.wowace));
            GetAddon("GridStatusMissingBuffs").Mappings.Add(new Mapping("grid-status-missing-buffs", AddonSiteId.curse));
            GetAddon("GridStatusMissingBuffs").Mappings.Add(new Mapping("grid-status-missing-buffs", AddonSiteId.wowace));
            GetAddon("GridStatusRaidIcons").Mappings.Add(new Mapping("grid-status-raid-icons", AddonSiteId.curse));
            GetAddon("GridStatusRaidIcons").Mappings.Add(new Mapping("grid-status-raid-icons", AddonSiteId.wowace));
            GetAddon("GridStatusReadyCheck").Mappings.Add(new Mapping("grid-status-ready-check", AddonSiteId.curse));
            GetAddon("GridStatusReadyCheck").Mappings.Add(new Mapping("grid-status-ready-check", AddonSiteId.wowace));
            GetAddon("GrimReaper").Mappings.Add(new Mapping("grim-reaper", AddonSiteId.curse));
            GetAddon("GrimReaper").Mappings.Add(new Mapping("grim-reaper", AddonSiteId.wowace));
            GetAddon("Hack").Mappings.Add(new Mapping("11101", AddonSiteId.wowinterface));
            GetAddon("Haggler").Mappings.Add(new Mapping("haggler", AddonSiteId.curse));
            GetAddon("Haggler").Mappings.Add(new Mapping("haggler", AddonSiteId.curseforge));
            GetAddon("HandyNotes").Mappings.Add(new Mapping("handynotes", AddonSiteId.curse));
            GetAddon("HandyNotes").Mappings.Add(new Mapping("handynotes", AddonSiteId.wowace));
            GetAddon("HandyNotes_FlightMasters").Mappings.Add(new Mapping("handy-notes_flight-masters", AddonSiteId.curse));
            GetAddon("HandyNotes_FlightMasters").Mappings.Add(new Mapping("handy-notes_flight-masters", AddonSiteId.wowace));
            GetAddon("HandyNotes_Guild").Mappings.Add(new Mapping("handynotes_guild", AddonSiteId.curse));
            GetAddon("HandyNotes_Guild").Mappings.Add(new Mapping("handynotes_guild", AddonSiteId.wowace));
            GetAddon("HandyNotes_Mailboxes").Mappings.Add(new Mapping("handy-notes_mailboxes", AddonSiteId.curse));
            GetAddon("HandyNotes_Mailboxes").Mappings.Add(new Mapping("handy-notes_mailboxes", AddonSiteId.wowace));
            GetAddon("HeadCount").Mappings.Add(new Mapping("head-count", AddonSiteId.curse));
            GetAddon("HeadCount").Mappings.Add(new Mapping("head-count", AddonSiteId.wowace));
            GetAddon("HealBot").Mappings.Add(new Mapping("heal-bot-continued", AddonSiteId.curse));
            GetAddon("HealBot").Mappings.Add(new Mapping("heal-bot-continued", AddonSiteId.curseforge));
            GetAddon("Heatsink").Mappings.Add(new Mapping("heatsink", AddonSiteId.curse));
            GetAddon("Heatsink").Mappings.Add(new Mapping("heatsink", AddonSiteId.wowace));
            GetAddon("HitsMode").Mappings.Add(new Mapping("hits-mode", AddonSiteId.curse));
            GetAddon("HitsMode").Mappings.Add(new Mapping("hits-mode", AddonSiteId.wowace));
            GetAddon("HKCounter").Mappings.Add(new Mapping("honor-kills-counter", AddonSiteId.curse));
            GetAddon("IceHUD").Mappings.Add(new Mapping("ice-hud", AddonSiteId.curse));
            GetAddon("IceHUD").Mappings.Add(new Mapping("ice-hud", AddonSiteId.wowace));
            GetAddon("IHML").Mappings.Add(new Mapping("ihml", AddonSiteId.curse));
            GetAddon("IHML").Mappings.Add(new Mapping("ihml", AddonSiteId.wowace));
            GetAddon("Incubator").Mappings.Add(new Mapping("incubator", AddonSiteId.curse));
            GetAddon("Incubator").Mappings.Add(new Mapping("incubator", AddonSiteId.wowace));
            GetAddon("InFlight").Mappings.Add(new Mapping("11178", AddonSiteId.wowinterface));
            GetAddon("InFlight").SubAddons.Add(GetAddon("InFlight_Load"));
            GetAddon("InstanceMaps").Mappings.Add(new Mapping("instance-maps", AddonSiteId.curse));
            GetAddon("InstanceMaps").Mappings.Add(new Mapping("instance-maps", AddonSiteId.wowace));
            GetAddon("ItemDB").Mappings.Add(new Mapping("itemdb", AddonSiteId.curse));
            GetAddon("ItemDB").Mappings.Add(new Mapping("itemdb", AddonSiteId.wowace));
            GetAddon("ItemPriceTooltip").Mappings.Add(new Mapping("item-price-tooltip", AddonSiteId.curse));
            GetAddon("ItemPriceTooltip").Mappings.Add(new Mapping("item-price-tooltip", AddonSiteId.wowace));
            GetAddon("ItemRack").Mappings.Add(new Mapping("4148", AddonSiteId.wowinterface));
            GetAddon("ItemRack").Mappings.Add(new Mapping("item-rack", AddonSiteId.curse));
            GetAddon("ItemRack").SubAddons.Add(GetAddon("ItemRackOptions"));
            GetAddon("JebusMail").Mappings.Add(new Mapping("jebus-mail", AddonSiteId.curse));
            GetAddon("JebusMail").Mappings.Add(new Mapping("jebus-mail", AddonSiteId.wowace));
            GetAddon("Junkie").Mappings.Add(new Mapping("junkie", AddonSiteId.curse));
            GetAddon("Junkie").Mappings.Add(new Mapping("junkie", AddonSiteId.curseforge));
            GetAddon("KC_Items").Mappings.Add(new Mapping("kc_items", AddonSiteId.curse));
            GetAddon("KC_Items").Mappings.Add(new Mapping("kc_items", AddonSiteId.wowace));
            GetAddon("kgPanels").Mappings.Add(new Mapping("kg-panels", AddonSiteId.curse));
            GetAddon("kgPanels").Mappings.Add(new Mapping("kg-panels", AddonSiteId.wowace));
            GetAddon("kgPanels").SubAddons.Add(GetAddon("kgPanelsConfig"));
            GetAddon("KillLog").Mappings.Add(new Mapping("kill-log-reloaded", AddonSiteId.curse));
            GetAddon("KillLog").Mappings.Add(new Mapping("kill-log-reloaded", AddonSiteId.curseforge));
            GetAddon("LD50_abar").Mappings.Add(new Mapping("ld50_abar", AddonSiteId.curse));
            GetAddon("LD50_abar").Mappings.Add(new Mapping("ld50_abar", AddonSiteId.wowace));
            GetAddon("LearningAid").Mappings.Add(new Mapping("learningaid", AddonSiteId.curse));
            GetAddon("LearningAid").Mappings.Add(new Mapping("learningaid", AddonSiteId.curseforge));
            GetAddon("lern2count").Mappings.Add(new Mapping("lern2count", AddonSiteId.curse));
            GetAddon("lern2count").Mappings.Add(new Mapping("lern2count", AddonSiteId.wowace));
            GetAddon("LightHeaded").Mappings.Add(new Mapping("7017", AddonSiteId.wowinterface));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_A"));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_B"));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_C"));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_D"));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_E"));
            GetAddon("Links").Mappings.Add(new Mapping("links", AddonSiteId.curse));
            GetAddon("Links").Mappings.Add(new Mapping("links", AddonSiteId.wowace));
            GetAddon("LinkWrangler").Mappings.Add(new Mapping("link-wrangler-1-7", AddonSiteId.curse));
            GetAddon("LinkWrangler").Mappings.Add(new Mapping("link-wrangler-1-7", AddonSiteId.curseforge));
            GetAddon("LitheTooltipDoctor").Mappings.Add(new Mapping("5336", AddonSiteId.wowinterface));
            GetAddon("LittleWigs").Mappings.Add(new Mapping("little-wigs", AddonSiteId.curse));
            GetAddon("LittleWigs").Mappings.Add(new Mapping("little-wigs", AddonSiteId.wowace));
            GetAddon("LittleWigs").SubAddons.Add(GetAddon("LittleWigs_Auchindoun"));
            GetAddon("LittleWigs").SubAddons.Add(GetAddon("LittleWigs_Coilfang"));
            GetAddon("LittleWigs").SubAddons.Add(GetAddon("LittleWigs_CoT"));
            GetAddon("LittleWigs").SubAddons.Add(GetAddon("LittleWigs_HellfireCitadel"));
            GetAddon("LittleWigs").SubAddons.Add(GetAddon("LittleWigs_MagistersTerrace"));
            GetAddon("LittleWigs").SubAddons.Add(GetAddon("LittleWigs_TempestKeep"));
            GetAddon("LuckyCharms2").Mappings.Add(new Mapping("4734", AddonSiteId.wowui));
            GetAddon("LuckyCharms2").Mappings.Add(new Mapping("lucky-charms2", AddonSiteId.curse));
            GetAddon("Ludwig").Mappings.Add(new Mapping("5174", AddonSiteId.wowinterface));
            GetAddon("LunarSphere").Mappings.Add(new Mapping("0.803|19.10.2008|http://moongazeaddons.proboards79.com/index.cgi?board=lunarsphere&action=display&thread=728|http://www.lunaraddons.com/LunarSphere_803.zip", AddonSiteId.direct));
            GetAddon("LunarSphere").SubAddons.Add(GetAddon("LunarSphereExporter"));
            GetAddon("LunarSphere").SubAddons.Add(GetAddon("LunarSphereImports"));
            GetAddon("Macaroon").Mappings.Add(new Mapping("10636", AddonSiteId.wowinterface));
            GetAddon("Macaroon").SubAddons.Add(GetAddon("MacaroonProfiles"));
            GetAddon("MacaroonXtras").Mappings.Add(new Mapping("10933", AddonSiteId.wowinterface));
            GetAddon("MacaroonXtras").SubAddons.Add(GetAddon("MacaroonBound"));
            GetAddon("MacaroonXtras").SubAddons.Add(GetAddon("MacaroonCB"));
            GetAddon("MacaroonXtras").SubAddons.Add(GetAddon("MacaroonLoot"));
            GetAddon("MacaroonXtras").SubAddons.Add(GetAddon("MacaroonXP"));
            GetAddon("MagicDKP").Mappings.Add(new Mapping("magic-dk", AddonSiteId.curse));
            GetAddon("MagicDKP").Mappings.Add(new Mapping("magic-dk", AddonSiteId.wowace));
            GetAddon("MainAssist").Mappings.Add(new Mapping("main-assist", AddonSiteId.curse));
            GetAddon("MainAssist").Mappings.Add(new Mapping("main-assist", AddonSiteId.wowace));
            GetAddon("Manufac").Mappings.Add(new Mapping("manufac", AddonSiteId.curse));
            GetAddon("Manufac").Mappings.Add(new Mapping("manufac", AddonSiteId.wowace));
            GetAddon("Mapster").Mappings.Add(new Mapping("mapster", AddonSiteId.curse));
            GetAddon("Mapster").Mappings.Add(new Mapping("mapster", AddonSiteId.wowace));
            GetAddon("MarsPartyBuff").Mappings.Add(new Mapping("MarsPartyBuff", AddonSiteId.wowspecial));
            GetAddon("MaterialsTracker").Mappings.Add(new Mapping("materials-tracker", AddonSiteId.curse));
            GetAddon("MaterialsTracker").Mappings.Add(new Mapping("materials-tracker", AddonSiteId.wowace));
            GetAddon("MBB").Mappings.Add(new Mapping("mbb", AddonSiteId.curse));
            GetAddon("MBB").Mappings.Add(new Mapping("mbb", AddonSiteId.wowace));
            GetAddon("Mendeleev").Mappings.Add(new Mapping("mendeleev", AddonSiteId.curse));
            GetAddon("Mendeleev").Mappings.Add(new Mapping("mendeleev", AddonSiteId.wowace));
            GetAddon("MikScrollingBattleText").Mappings.Add(new Mapping("5153", AddonSiteId.wowinterface));
            GetAddon("MikScrollingBattleText").Mappings.Add(new Mapping("mik-scrolling-battle-text", AddonSiteId.curse));
            GetAddon("MikScrollingBattleText").SubAddons.Add(GetAddon("MSBTOptions"));
            GetAddon("Minimalisque_v3").Mappings.Add(new Mapping("11184", AddonSiteId.wowinterface));
            GetAddon("Minimalist").Mappings.Add(new Mapping("minimalist", AddonSiteId.curse));
            GetAddon("Minimalist").Mappings.Add(new Mapping("minimalist", AddonSiteId.wowace));
            GetAddon("MinimapButtonFrame").Mappings.Add(new Mapping("7929", AddonSiteId.wowinterface));
            GetAddon("MinimapButtonFrame").SubAddons.Add(GetAddon("MinimapButtonFrame_SkinPack"));
            GetAddon("MinimapButtonFrame").SubAddons.Add(GetAddon("MinimapButtonFrameFu"));
            GetAddon("MinimapButtonFrame").SubAddons.Add(GetAddon("MinimapButtonFrameTitanPlugin"));
            GetAddon("MiniMapster").Mappings.Add(new Mapping("minimapster", AddonSiteId.curse));
            GetAddon("MiniMapster").Mappings.Add(new Mapping("minimapster", AddonSiteId.wowace));
            GetAddon("Mirror").Mappings.Add(new Mapping("9674", AddonSiteId.wowinterface));
            GetAddon("Mirror").Mappings.Add(new Mapping("mirror", AddonSiteId.curse));
            GetAddon("Mirror").Mappings.Add(new Mapping("mirror", AddonSiteId.wowace));
            GetAddon("MobHealth").Mappings.Add(new Mapping("mob-health", AddonSiteId.curse));
            GetAddon("MobHealth").Mappings.Add(new Mapping("mob-health", AddonSiteId.wowace));
            GetAddon("MobHealth3_BlizzardFrames").Mappings.Add(new Mapping("mob-health3_blizzard-frames", AddonSiteId.curse));
            GetAddon("MobHealth3_BlizzardFrames").Mappings.Add(new Mapping("mob-health3_blizzard-frames", AddonSiteId.wowace));
            GetAddon("MobMap").Mappings.Add(new Mapping("MobMap", AddonSiteId.wowspecial));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub1"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub10"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub11"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub12"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub13"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub14"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub2"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub3"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub4"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub5"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub6"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub7"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub8"));
            GetAddon("MobMap").SubAddons.Add(GetAddon("MobMapDatabaseStub9"));
            GetAddon("MobSpells").Mappings.Add(new Mapping("mob-spells", AddonSiteId.curse));
            GetAddon("MobSpells").Mappings.Add(new Mapping("mob-spells", AddonSiteId.wowace));
            GetAddon("MonkeyBuddy").Mappings.Add(new Mapping("monkey-buddy", AddonSiteId.curse));
            GetAddon("MonkeyBuddy").Mappings.Add(new Mapping("monkey-buddy", AddonSiteId.curseforge));
            GetAddon("MonkeyQuest").Mappings.Add(new Mapping("monkey-quest", AddonSiteId.curse));
            GetAddon("MonkeyQuest").Mappings.Add(new Mapping("monkey-quest", AddonSiteId.curseforge));
            GetAddon("MonkeyQuest").SubAddons.Add(GetAddon("MonkeyLibrary"));
            GetAddon("MonkeyQuest").SubAddons.Add(GetAddon("MonkeyQuestLog"));
            GetAddon("MoveAnything").Mappings.Add(new Mapping("11208", AddonSiteId.wowinterface));
            GetAddon("MTLove").Mappings.Add(new Mapping("7024", AddonSiteId.wowinterface));
            GetAddon("naiCombo").Mappings.Add(new Mapping("8913", AddonSiteId.wowinterface));
            GetAddon("Necrosis").Mappings.Add(new Mapping("necrosis-ld-c", AddonSiteId.curse));
            GetAddon("Niagara").Mappings.Add(new Mapping("niagara", AddonSiteId.curse));
            GetAddon("Niagara").Mappings.Add(new Mapping("niagara", AddonSiteId.wowace));
            GetAddon("NowCarrying").Mappings.Add(new Mapping("10172", AddonSiteId.wowinterface));
            GetAddon("nQuestLog").Mappings.Add(new Mapping("n-quest-log", AddonSiteId.curse));
            GetAddon("nQuestLog").Mappings.Add(new Mapping("n-quest-log", AddonSiteId.wowace));
            GetAddon("NugEnergy").Mappings.Add(new Mapping("9099", AddonSiteId.wowinterface));
            GetAddon("Numen").Mappings.Add(new Mapping("numen", AddonSiteId.curse));
            GetAddon("Numen").Mappings.Add(new Mapping("numen", AddonSiteId.wowace));
            GetAddon("oGlow").Mappings.Add(new Mapping("7142", AddonSiteId.wowinterface));
            GetAddon("Omen").Mappings.Add(new Mapping("omen-threat-meter", AddonSiteId.curse));
            GetAddon("Omen").Mappings.Add(new Mapping("omen-threat-meter", AddonSiteId.wowace));
            GetAddon("OmniCC").Mappings.Add(new Mapping("4836", AddonSiteId.wowinterface));
            GetAddon("OmniCC").Mappings.Add(new Mapping("omni-cc", AddonSiteId.curse));
            GetAddon("OmniCC").SubAddons.Add(GetAddon("OmniCC_Options"));
            GetAddon("OneBag3").Mappings.Add(new Mapping("onebag3", AddonSiteId.curse));
            GetAddon("OneBag3").Mappings.Add(new Mapping("onebag3", AddonSiteId.wowace));
            GetAddon("OneClickBuyOut").Mappings.Add(new Mapping("ocbo", AddonSiteId.curse));
            GetAddon("OneClickBuyOut").Mappings.Add(new Mapping("ocbo", AddonSiteId.curseforge));
            GetAddon("OPie").Mappings.Add(new Mapping("9094", AddonSiteId.wowinterface));
            GetAddon("OptiTaunt").Mappings.Add(new Mapping("opti-taunt", AddonSiteId.curse));
            GetAddon("OptiTaunt").Mappings.Add(new Mapping("opti-taunt", AddonSiteId.wowace));
            GetAddon("oRA2").Mappings.Add(new Mapping("ora2", AddonSiteId.curse));
            GetAddon("oRA2").Mappings.Add(new Mapping("ora2", AddonSiteId.wowace));
            GetAddon("oRA2").SubAddons.Add(GetAddon("oRA2_Leader"));
            GetAddon("oRA2").SubAddons.Add(GetAddon("oRA2_Optional"));
            GetAddon("oRA2").SubAddons.Add(GetAddon("oRA2_Participant"));
            GetAddon("Outfitter").Mappings.Add(new Mapping("outfitter", AddonSiteId.curse));
            GetAddon("Overachiever").Mappings.Add(new Mapping("overachiever", AddonSiteId.curse));
            GetAddon("Overachiever").Mappings.Add(new Mapping("overachiever", AddonSiteId.curseforge));
            GetAddon("Parrot").Mappings.Add(new Mapping("parrot", AddonSiteId.curse));
            GetAddon("Parrot").Mappings.Add(new Mapping("parrot", AddonSiteId.wowace));
            GetAddon("PartySpotter").Mappings.Add(new Mapping("4389", AddonSiteId.wowinterface));
            GetAddon("PartySpotter").Mappings.Add(new Mapping("party-spotter", AddonSiteId.curse));
            GetAddon("PartySpotter").Mappings.Add(new Mapping("party-spotter", AddonSiteId.curseforge));
            GetAddon("picoDPS").Mappings.Add(new Mapping("10573", AddonSiteId.wowinterface));
            GetAddon("picoEXP").Mappings.Add(new Mapping("10259", AddonSiteId.wowinterface));
            GetAddon("picoFriends").Mappings.Add(new Mapping("9220", AddonSiteId.wowinterface));
            GetAddon("picoGuild").Mappings.Add(new Mapping("9219", AddonSiteId.wowinterface));
            GetAddon("PitBull").Mappings.Add(new Mapping("pit-bull", AddonSiteId.curse));
            GetAddon("PitBull").Mappings.Add(new Mapping("pit-bull", AddonSiteId.wowace));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_Aura"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_Banzai"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_BarFader"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_CastBar"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_CombatFader"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_CombatIcon"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_CombatText"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_ComboPoints"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_DruidManaBar"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_ExperienceBar"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_HappinessIcon"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_HealthBar"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_HideBlizzard"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_Highlight"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_LeaderIcon"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_MasterLooterIcon"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_Portrait"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_PowerBar"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_PvPIcon"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_RaidTargetIcon"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_RangeCheck"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_ReadyCheckIcon"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_ReputationBar"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_RestIcon"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_Spark"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_ThreatBar"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_TotemTimers"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_VisualHeal"));
            GetAddon("PitBull").SubAddons.Add(GetAddon("PitBull_VoiceIcon"));
            GetAddon("PitBull_QuickHealth").Mappings.Add(new Mapping("pit-bull_quick-health", AddonSiteId.curse));
            GetAddon("PitBull_QuickHealth").Mappings.Add(new Mapping("pit-bull_quick-health", AddonSiteId.wowace));
            GetAddon("PoliteWhisper").Mappings.Add(new Mapping("politewhisperfan", AddonSiteId.curse));
            GetAddon("PoliteWhisper").Mappings.Add(new Mapping("powr2", AddonSiteId.curse));
            GetAddon("Possessions").Mappings.Add(new Mapping("6443", AddonSiteId.wowinterface));
            GetAddon("Postal").Mappings.Add(new Mapping("postal", AddonSiteId.curse));
            GetAddon("Postal").Mappings.Add(new Mapping("postal", AddonSiteId.wowace));
            GetAddon("Prat-3.0").Mappings.Add(new Mapping("prat-3-0", AddonSiteId.curse));
            GetAddon("Prat-3.0").Mappings.Add(new Mapping("prat-3-0", AddonSiteId.wowace));
            GetAddon("Prat-3.0").SubAddons.Add(GetAddon("Prat-3.0_HighCPUUsageModules"));
            GetAddon("Prat-3.0").SubAddons.Add(GetAddon("Prat-3.0_Libraries"));
            GetAddon("PreformAvEnabler").Mappings.Add(new Mapping("preform-av-enabler", AddonSiteId.curse));
            GetAddon("ProfessionsBook").Mappings.Add(new Mapping("professions-book", AddonSiteId.curse));
            GetAddon("ProfessionsBook").Mappings.Add(new Mapping("professions-book", AddonSiteId.wowace));
            GetAddon("pRogue").Mappings.Add(new Mapping("8560", AddonSiteId.wowinterface));
            GetAddon("Proximo").Mappings.Add(new Mapping("proximo", AddonSiteId.curse));
            GetAddon("Proximo").Mappings.Add(new Mapping("proximo", AddonSiteId.wowace));
            GetAddon("Quartz").Mappings.Add(new Mapping("quartz", AddonSiteId.curse));
            GetAddon("Quartz").Mappings.Add(new Mapping("quartz", AddonSiteId.wowace));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Buff"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Flight"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Focus"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_GCD"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Interrupt"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Latency"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Mirror"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Pet"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Player"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Range"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Swing"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Target"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Timer"));
            GetAddon("Quartz").SubAddons.Add(GetAddon("Quartz_Tradeskill"));
            GetAddon("QuesterJester").Mappings.Add(new Mapping("11005", AddonSiteId.wowinterface));
            GetAddon("QuestGuru").Mappings.Add(new Mapping("4855", AddonSiteId.wowui));
            GetAddon("QuestGuru").Mappings.Add(new Mapping("quest-guru", AddonSiteId.curse));
            GetAddon("QuestHelper").Mappings.Add(new Mapping("6271", AddonSiteId.wowui));
            GetAddon("QuestHelper").Mappings.Add(new Mapping("9896", AddonSiteId.wowinterface));
            GetAddon("QuestHelper").Mappings.Add(new Mapping("quest-helper", AddonSiteId.curse));
            GetAddon("QuestHelper").Mappings.Add(new Mapping("quest-helper", AddonSiteId.curseforge));
            GetAddon("QuickStatsV2").Mappings.Add(new Mapping("quick-stats-v2", AddonSiteId.curse));
            GetAddon("Quixote").Mappings.Add(new Mapping("quixote", AddonSiteId.curse));
            GetAddon("Quixote").Mappings.Add(new Mapping("quixote", AddonSiteId.wowace));
            GetAddon("RaidBuffStatus").Mappings.Add(new Mapping("raidbuffstatus", AddonSiteId.curse));
            GetAddon("RaidBuffStatus").Mappings.Add(new Mapping("raidbuffstatus", AddonSiteId.wowace));
            GetAddon("RangeDisplay").Mappings.Add(new Mapping("range-display", AddonSiteId.curse));
            GetAddon("RangeDisplay").Mappings.Add(new Mapping("range-display", AddonSiteId.wowace));
            GetAddon("RangeRecolor").Mappings.Add(new Mapping("range-recolor", AddonSiteId.curse));
            GetAddon("RangeRecolor").Mappings.Add(new Mapping("range-recolor", AddonSiteId.wowace));
            GetAddon("RatingBuster").Mappings.Add(new Mapping("rating-buster", AddonSiteId.curse));
            GetAddon("RatingBuster").Mappings.Add(new Mapping("rating-buster", AddonSiteId.wowace));
            GetAddon("ReagentRestocker").Mappings.Add(new Mapping("7569", AddonSiteId.wowinterface));
            GetAddon("Recount").Mappings.Add(new Mapping("recount", AddonSiteId.curse));
            GetAddon("Recount").Mappings.Add(new Mapping("recount", AddonSiteId.wowace));
            GetAddon("RedRange").Mappings.Add(new Mapping("4166", AddonSiteId.wowinterface));
            GetAddon("Rep2").Mappings.Add(new Mapping("rep2", AddonSiteId.curse));
            GetAddon("Rep2").Mappings.Add(new Mapping("rep2", AddonSiteId.wowace));
            GetAddon("Reslack").Mappings.Add(new Mapping("reslack", AddonSiteId.curse));
            GetAddon("Reslack").Mappings.Add(new Mapping("reslack", AddonSiteId.wowace));
            GetAddon("RightWing").Mappings.Add(new Mapping("right-wing", AddonSiteId.curse));
            GetAddon("RightWing").Mappings.Add(new Mapping("right-wing", AddonSiteId.wowace));
            GetAddon("RogueFocus").Mappings.Add(new Mapping("6060", AddonSiteId.wowui));
            GetAddon("RosterLib").Mappings.Add(new Mapping("rosterlib", AddonSiteId.curse));
            GetAddon("RosterLib").Mappings.Add(new Mapping("rosterlib", AddonSiteId.wowace));
            GetAddon("Routes").Mappings.Add(new Mapping("routes", AddonSiteId.curse));
            GetAddon("Routes").Mappings.Add(new Mapping("routes", AddonSiteId.wowace));
            GetAddon("rSelfCast").Mappings.Add(new Mapping("r-self-cast", AddonSiteId.curse));
            GetAddon("rSelfCast").Mappings.Add(new Mapping("r-self-cast", AddonSiteId.wowace));
            GetAddon("SalvationKiller").Mappings.Add(new Mapping("salvation-killer", AddonSiteId.curse));
            GetAddon("SalvationKiller").Mappings.Add(new Mapping("salvation-killer", AddonSiteId.wowace));
            GetAddon("sct").Mappings.Add(new Mapping("sct", AddonSiteId.curse));
            GetAddon("sct").Mappings.Add(new Mapping("sct", AddonSiteId.wowace));
            GetAddon("sct_options").Mappings.Add(new Mapping("sct_options", AddonSiteId.curse));
            GetAddon("sct_options").Mappings.Add(new Mapping("sct_options", AddonSiteId.wowace));
            GetAddon("sctd").Mappings.Add(new Mapping("sct-damage", AddonSiteId.curse));
            GetAddon("sctd").SubAddons.Add(GetAddon("sctd_options"));
            GetAddon("SellFish").Mappings.Add(new Mapping("sell-fish", AddonSiteId.curse));
            GetAddon("SellFish").Mappings.Add(new Mapping("sell-fish", AddonSiteId.curseforge));
            GetAddon("SexyMap").Mappings.Add(new Mapping("sexymap", AddonSiteId.curse));
            GetAddon("SexyMap").Mappings.Add(new Mapping("sexymap", AddonSiteId.wowace));
            GetAddon("SharedMedia").Mappings.Add(new Mapping("sharedmedia", AddonSiteId.curse));
            GetAddon("SharedMedia").Mappings.Add(new Mapping("sharedmedia", AddonSiteId.wowace));
            GetAddon("SharedMediaAdditionalFonts").Mappings.Add(new Mapping("shared-media-additional-fonts", AddonSiteId.curse));
            GetAddon("SharedMediaAdditionalFonts").Mappings.Add(new Mapping("shared-media-additional-fonts", AddonSiteId.wowace));
            GetAddon("ShockAndAwe").Mappings.Add(new Mapping("shockandawe", AddonSiteId.curse));
            GetAddon("ShockAndAwe").Mappings.Add(new Mapping("shockandawe", AddonSiteId.curseforge));
            GetAddon("SickOfClickingDailies").Mappings.Add(new Mapping("sick-of-clicking-dailies", AddonSiteId.curse));
            GetAddon("SickOfClickingDailies").Mappings.Add(new Mapping("sick-of-clicking-dailies", AddonSiteId.wowace));
            GetAddon("SimpleBuffBars").Mappings.Add(new Mapping("9798", AddonSiteId.wowinterface));
            GetAddon("simpleMinimap").Mappings.Add(new Mapping("simple-minimap", AddonSiteId.curse));
            GetAddon("simpleMinimap").Mappings.Add(new Mapping("simple-minimap", AddonSiteId.wowace));
            GetAddon("SimpleRaidTargetIcons").Mappings.Add(new Mapping("simple-raid-target-icons", AddonSiteId.curse));
            GetAddon("SimpleSelfRebuff").Mappings.Add(new Mapping("simple-self-rebuff", AddonSiteId.curse));
            GetAddon("SimpleSelfRebuff").Mappings.Add(new Mapping("simple-self-rebuff", AddonSiteId.wowace));
            GetAddon("SimpleSelfRebuff").SubAddons.Add(GetAddon("SimpleSelfRebuff_CastBinding"));
            GetAddon("SimpleSelfRebuff").SubAddons.Add(GetAddon("SimpleSelfRebuff_DataObject"));
            GetAddon("SimpleSelfRebuff").SubAddons.Add(GetAddon("SimpleSelfRebuff_ItemBuffs"));
            GetAddon("SimpleSelfRebuff").SubAddons.Add(GetAddon("SimpleSelfRebuff_Reminder"));
            GetAddon("Skillet").Mappings.Add(new Mapping("skillet", AddonSiteId.curse));
            GetAddon("Skillet").Mappings.Add(new Mapping("skillet", AddonSiteId.wowace));
            GetAddon("Skinner").Mappings.Add(new Mapping("skinner", AddonSiteId.curse));
            GetAddon("Skinner").Mappings.Add(new Mapping("skinner", AddonSiteId.wowace));
            GetAddon("SlashConflict").Mappings.Add(new Mapping("slash-conflict", AddonSiteId.curse));
            GetAddon("SlashConflict").Mappings.Add(new Mapping("slash-conflict", AddonSiteId.wowace));
            GetAddon("SLDataText").Mappings.Add(new Mapping("8539", AddonSiteId.wowinterface));
            GetAddon("SmartBuff").Mappings.Add(new Mapping("smart-buff", AddonSiteId.curse));
            GetAddon("SmartBuff").Mappings.Add(new Mapping("smart-buff", AddonSiteId.curseforge));
            GetAddon("SmartMount").Mappings.Add(new Mapping("smartmount", AddonSiteId.curse));
            GetAddon("SmartMount").Mappings.Add(new Mapping("smartmount", AddonSiteId.wowace));
            GetAddon("SnapshotBar").Mappings.Add(new Mapping("snapshot-bar", AddonSiteId.curse));
            GetAddon("SnapshotBar").Mappings.Add(new Mapping("snapshot-bar", AddonSiteId.wowace));
            GetAddon("Snoopy").Mappings.Add(new Mapping("11180", AddonSiteId.wowinterface));
            GetAddon("SpamMeNot").Mappings.Add(new Mapping("spam-me-not", AddonSiteId.curse));
            GetAddon("SpecialEventsEmbed").Mappings.Add(new Mapping("specialeventsembed", AddonSiteId.curse));
            GetAddon("SpecialEventsEmbed").Mappings.Add(new Mapping("specialeventsembed", AddonSiteId.wowace));
            GetAddon("Sphere").Mappings.Add(new Mapping("sphere", AddonSiteId.curse));
            GetAddon("Sphere").Mappings.Add(new Mapping("sphere", AddonSiteId.wowace));
            GetAddon("Spyglass").Mappings.Add(new Mapping("spyglass", AddonSiteId.curse));
            GetAddon("Spyglass").Mappings.Add(new Mapping("spyglass", AddonSiteId.wowace));
            GetAddon("sRaidFrames").Mappings.Add(new Mapping("sraidframes", AddonSiteId.curse));
            GetAddon("sRaidFrames").Mappings.Add(new Mapping("sraidframes", AddonSiteId.wowace));
            GetAddon("SSPVP3").Mappings.Add(new Mapping("4569", AddonSiteId.wowinterface));
            GetAddon("startip").Mappings.Add(new Mapping("startip", AddonSiteId.curse));
            GetAddon("startip").Mappings.Add(new Mapping("startip", AddonSiteId.curseforge));
            GetAddon("StatBlock_Ammo").Mappings.Add(new Mapping("9222", AddonSiteId.wowinterface));
            GetAddon("StatBlock_Ammo").Mappings.Add(new Mapping("stat-block_ammo", AddonSiteId.curse));
            GetAddon("StatBlock_Ammo").Mappings.Add(new Mapping("stat-block_ammo", AddonSiteId.wowace));
            GetAddon("StatBlock_DPS").Mappings.Add(new Mapping("stat-block_dps", AddonSiteId.curse));
            GetAddon("StatBlock_DPS").Mappings.Add(new Mapping("stat-block_dps", AddonSiteId.wowace));
            GetAddon("StatBlock_Durability").Mappings.Add(new Mapping("stat-block_durability", AddonSiteId.curse));
            GetAddon("StatBlock_Durability").Mappings.Add(new Mapping("stat-block_durability", AddonSiteId.wowace));
            GetAddon("StatBlock_Factions").Mappings.Add(new Mapping("stat-block_factions", AddonSiteId.curse));
            GetAddon("StatBlock_Factions").Mappings.Add(new Mapping("stat-block_factions", AddonSiteId.wowace));
            GetAddon("StatBlock_FPS").Mappings.Add(new Mapping("stat-block_fps", AddonSiteId.curse));
            GetAddon("StatBlock_FPS").Mappings.Add(new Mapping("stat-block_fps", AddonSiteId.wowace));
            GetAddon("StatBlock_Latency").Mappings.Add(new Mapping("stat-block_latency", AddonSiteId.curse));
            GetAddon("StatBlock_Latency").Mappings.Add(new Mapping("stat-block_latency", AddonSiteId.wowace));
            GetAddon("StatBlock_Memory").Mappings.Add(new Mapping("stat-block_memory", AddonSiteId.curse));
            GetAddon("StatBlock_Memory").Mappings.Add(new Mapping("stat-block_memory", AddonSiteId.wowace));
            GetAddon("StatBlock_Money").Mappings.Add(new Mapping("stat-block_money", AddonSiteId.curse));
            GetAddon("StatBlock_Money").Mappings.Add(new Mapping("stat-block_money", AddonSiteId.wowace));
            GetAddon("StatBlock_RaidDPS").Mappings.Add(new Mapping("stat-block_raid-dps", AddonSiteId.curse));
            GetAddon("StatBlock_RaidDPS").Mappings.Add(new Mapping("stat-block_raid-dps", AddonSiteId.wowace));
            GetAddon("StatBlock_ZoneText").Mappings.Add(new Mapping("stat-block_zone-text", AddonSiteId.curse));
            GetAddon("StatBlock_ZoneText").Mappings.Add(new Mapping("stat-block_zone-text", AddonSiteId.wowace));
            GetAddon("StatBlockCore").Mappings.Add(new Mapping("stat-block-core", AddonSiteId.curse));
            GetAddon("StatBlockCore").Mappings.Add(new Mapping("stat-block-core", AddonSiteId.wowace));
            GetAddon("StatLogicLib").Mappings.Add(new Mapping("statlogiclib", AddonSiteId.curse));
            GetAddon("StatLogicLib").Mappings.Add(new Mapping("statlogiclib", AddonSiteId.wowace));
            GetAddon("SW_Stats").Mappings.Add(new Mapping("sw-stats", AddonSiteId.curse));
            GetAddon("SW_Stats").SubAddons.Add(GetAddon("SW_Stats_Profiles"));
            GetAddon("SW_Stats").SubAddons.Add(GetAddon("SW_UniLog"));
            GetAddon("Talented").Mappings.Add(new Mapping("talented", AddonSiteId.curse));
            GetAddon("Talented").Mappings.Add(new Mapping("talented", AddonSiteId.wowace));
            GetAddon("Talented_Data").Mappings.Add(new Mapping("talented_data", AddonSiteId.curse));
            GetAddon("Talented_Data").Mappings.Add(new Mapping("talented_data", AddonSiteId.wowace));
            GetAddon("Talented_Loader").Mappings.Add(new Mapping("talented_loader", AddonSiteId.curse));
            GetAddon("Talented_Loader").Mappings.Add(new Mapping("talented_loader", AddonSiteId.wowace));
            GetAddon("TankWarnings").Mappings.Add(new Mapping("tankwarnings", AddonSiteId.curse));
            GetAddon("TankWarnings").Mappings.Add(new Mapping("tankwarnings", AddonSiteId.wowace));
            GetAddon("teksLoot").Mappings.Add(new Mapping("8198", AddonSiteId.wowinterface));
            GetAddon("texbrowser").Mappings.Add(new Mapping("texbrowser", AddonSiteId.curse));
            GetAddon("texbrowser").Mappings.Add(new Mapping("texbrowser", AddonSiteId.wowace));
            GetAddon("Tipachu").Mappings.Add(new Mapping("8808", AddonSiteId.wowinterface));
            GetAddon("TipTac").Mappings.Add(new Mapping("tip-tac", AddonSiteId.curse));
            GetAddon("TipTac").Mappings.Add(new Mapping("tip-tac", AddonSiteId.curseforge));
            GetAddon("TipTac").SubAddons.Add(GetAddon("TipTacOptions"));
            GetAddon("TipTac").SubAddons.Add(GetAddon("TipTacTalents"));
            GetAddon("TipTop").Mappings.Add(new Mapping("10627", AddonSiteId.wowinterface));
            GetAddon("TomTom").Mappings.Add(new Mapping("7032", AddonSiteId.wowinterface));
            GetAddon("TotemManager").Mappings.Add(new Mapping("10080", AddonSiteId.wowinterface));
            GetAddon("TotemTimers").Mappings.Add(new Mapping("totemtimers", AddonSiteId.curse));
            GetAddon("TotemTimers").Mappings.Add(new Mapping("totemtimers", AddonSiteId.curseforge));
            GetAddon("TourGuide").Mappings.Add(new Mapping("7674", AddonSiteId.wowinterface));
            GetAddon("TouristLib").Mappings.Add(new Mapping("touristlib", AddonSiteId.curse));
            GetAddon("TouristLib").Mappings.Add(new Mapping("touristlib", AddonSiteId.wowace));
            GetAddon("Transcriptor").Mappings.Add(new Mapping("transcriptor", AddonSiteId.curse));
            GetAddon("Transcriptor").Mappings.Add(new Mapping("transcriptor", AddonSiteId.wowace));
            GetAddon("TrinketMenu").Mappings.Add(new Mapping("trinket-menu", AddonSiteId.curse));
            GetAddon("UnderHood").Mappings.Add(new Mapping("under-hood", AddonSiteId.curse));
            GetAddon("UnderHood").Mappings.Add(new Mapping("under-hood", AddonSiteId.wowace));
            GetAddon("UrbanAchiever").Mappings.Add(new Mapping("10867", AddonSiteId.wowinterface));
            GetAddon("Venantes").Mappings.Add(new Mapping("4692", AddonSiteId.wowui));
            GetAddon("Venantes").SubAddons.Add(GetAddon("SphereLoader"));
            GetAddon("Venantes").SubAddons.Add(GetAddon("VenantesOptions"));
            GetAddon("VendorBait").Mappings.Add(new Mapping("7121", AddonSiteId.wowinterface));
            GetAddon("Violation").Mappings.Add(new Mapping("violation", AddonSiteId.curse));
            GetAddon("Violation").Mappings.Add(new Mapping("violation", AddonSiteId.wowace));
            GetAddon("VuhDo").Mappings.Add(new Mapping("vuhdo", AddonSiteId.curse));
            GetAddon("VuhDo").Mappings.Add(new Mapping("vuhdo", AddonSiteId.curseforge));
            GetAddon("WeaponRebuff").Mappings.Add(new Mapping("weapon-rebuff", AddonSiteId.curse));
            GetAddon("WeaponRebuff").Mappings.Add(new Mapping("weapon-rebuff", AddonSiteId.wowace));
            GetAddon("WhoHas").Mappings.Add(new Mapping("who-has", AddonSiteId.curse));
            GetAddon("WhoLib").Mappings.Add(new Mapping("wholib", AddonSiteId.curse));
            GetAddon("WhoLib").Mappings.Add(new Mapping("wholib", AddonSiteId.wowace));
            GetAddon("WIM").Mappings.Add(new Mapping("wim-wo-w-instant-messenger", AddonSiteId.curse));
            GetAddon("WIM").SubAddons.Add(GetAddon("WIM_Options"));
            GetAddon("WindowLib").Mappings.Add(new Mapping("windowlib", AddonSiteId.curse));
            GetAddon("WindowLib").Mappings.Add(new Mapping("windowlib", AddonSiteId.wowace));
            GetAddon("WitchHunt").Mappings.Add(new Mapping("witch-hunt", AddonSiteId.curse));
            GetAddon("WitchHunt").Mappings.Add(new Mapping("witch-hunt", AddonSiteId.wowace));
            GetAddon("WoWEquip").Mappings.Add(new Mapping("wowequip", AddonSiteId.curse));
            GetAddon("WoWEquip").Mappings.Add(new Mapping("wowequip", AddonSiteId.wowace));
            GetAddon("xchar").Mappings.Add(new Mapping("xchar", AddonSiteId.wowspecial));
            GetAddon("xchar_Arygos").Mappings.Add(new Mapping("xchar_Arygos", AddonSiteId.wowspecial));
            GetAddon("xchar_Kargath").Mappings.Add(new Mapping("xchar_Kargath", AddonSiteId.wowspecial));
            GetAddon("xchar_MalGanis").Mappings.Add(new Mapping("xchar_MalGanis", AddonSiteId.wowspecial));
            GetAddon("XLoot").Mappings.Add(new Mapping("xloot", AddonSiteId.curse));
            GetAddon("XLoot").Mappings.Add(new Mapping("xloot", AddonSiteId.wowace));
            GetAddon("XLootGroup").Mappings.Add(new Mapping("xloot-group", AddonSiteId.curse));
            GetAddon("XLootGroup").Mappings.Add(new Mapping("xloot-group", AddonSiteId.wowace));
            GetAddon("XLootMaster").Mappings.Add(new Mapping("xloot-master", AddonSiteId.curse));
            GetAddon("XLootMaster").Mappings.Add(new Mapping("xloot-master", AddonSiteId.wowace));
            GetAddon("XLootMonitor").Mappings.Add(new Mapping("xloot-monitor", AddonSiteId.curse));
            GetAddon("XLootMonitor").Mappings.Add(new Mapping("xloot-monitor", AddonSiteId.wowace));
            GetAddon("XPerl").Mappings.Add(new Mapping("xperl", AddonSiteId.curse));
            GetAddon("XPerl").Mappings.Add(new Mapping("xperl", AddonSiteId.wowace));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_ArcaneBar"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_CustomHighlight"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_Options"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_Party"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_PartyPet"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_Player"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_PlayerBuffs"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_PlayerPet"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_RaidAdmin"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_RaidFrames"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_RaidHelper"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_RaidMonitor"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_RaidPets"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_Target"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_TargetTarget"));
            GetAddon("XPerl").SubAddons.Add(GetAddon("XPerl_Tutorial"));
            GetAddon("XRS").Mappings.Add(new Mapping("xrs", AddonSiteId.curse));
            GetAddon("XRS").Mappings.Add(new Mapping("xrs", AddonSiteId.wowace));
            GetAddon("Yata").Mappings.Add(new Mapping("yata", AddonSiteId.curse));
            GetAddon("Yata").Mappings.Add(new Mapping("yata", AddonSiteId.wowace));
            GetAddon("Yatba").Mappings.Add(new Mapping("yatba", AddonSiteId.curse));
            GetAddon("Yatba").Mappings.Add(new Mapping("yatba", AddonSiteId.wowace));
            GetAddon("YurrCombatLog").Mappings.Add(new Mapping("yurr-combat-log", AddonSiteId.curse));
            GetAddon("YurrCombatLog").Mappings.Add(new Mapping("yurr-combat-log", AddonSiteId.wowace));
            GetAddon("ZHunterMod").Mappings.Add(new Mapping("zhunter-mod", AddonSiteId.curse));
            GetAddon("ZOMGBuffs").Mappings.Add(new Mapping("zomgbuffs", AddonSiteId.curse));
            GetAddon("ZOMGBuffs").Mappings.Add(new Mapping("zomgbuffs", AddonSiteId.wowace));

            GetAddon("Broker_Location").Mappings.Add(new Mapping("broker_location", AddonSiteId.curse));
            GetAddon("Broker_Location").Mappings.Add(new Mapping("broker_location", AddonSiteId.curseforge));
            GetAddon("SexyMap").Mappings.Add(new Mapping("sexymap", AddonSiteId.wowace));
            GetAddon("SexyMap").Mappings.Add(new Mapping("sexymap", AddonSiteId.curse));
            GetAddon("texbrowser").Mappings.Add(new Mapping("texbrowser", AddonSiteId.wowace));
            GetAddon("texbrowser").Mappings.Add(new Mapping("texbrowser", AddonSiteId.curse));
            GetAddon("Examiner").Mappings.Add(new Mapping("7377", AddonSiteId.wowinterface));
            GetAddon("LightHeaded").Mappings.Add(new Mapping("7017", AddonSiteId.wowinterface));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_A"));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_B"));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_C"));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_D"));
            GetAddon("LightHeaded").SubAddons.Add(GetAddon("LightHeaded_Data_E"));


            #region Auctioneer
            // AuctioneerSuite
            GetPackage("AuctioneerSuite").Mappings.Add(new Mapping("5.1.3715|24.10.2008|http://auctioneeraddon.com/dl/#release|http://mirror.auctioneeraddon.com/dl/Packages3/AuctioneerSuite-5.1.3715.zip", AddonSiteId.direct));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("!Swatter"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Auc-Advanced"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Auc-Filter-Basic"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Auc-ScanData"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Auc-Stat-Classic"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Auc-Stat-Histogram"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Auc-Stat-iLevel"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Auc-Stat-Purchased"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Auc-Stat-Simple"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Auc-Stat-StdDev"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("BeanCounter"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Enchantrix"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Enchantrix-Barker"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("EnhTooltip"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Informant"));
            GetPackage("AuctioneerSuite").Addons.Add(GetAddon("Stubby"));

            // Auctioneer
            GetPackage("Auctioneer").Mappings.Add(new Mapping("5.1.3715|24.10.2008|http://auctioneeraddon.com/dl/#release|http://mirror.auctioneeraddon.com/dl/Packages3/Auctioneer-5.1.3715.zip", AddonSiteId.direct));
            GetPackage("Auctioneer").Addons.Add(GetAddon("!Swatter"));
            GetPackage("Auctioneer").Addons.Add(GetAddon("Auc-Advanced"));
            GetPackage("Auctioneer").Addons.Add(GetAddon("Auc-ScanData"));
            GetPackage("Auctioneer").Addons.Add(GetAddon("Auc-Stat-Histogram"));
            GetPackage("Auctioneer").Addons.Add(GetAddon("Auc-Stat-iLevel"));
            GetPackage("Auctioneer").Addons.Add(GetAddon("Auc-Stat-StdDev"));
            GetPackage("Auctioneer").Addons.Add(GetAddon("EnhTooltip"));
            GetPackage("Auctioneer").Addons.Add(GetAddon("Stubby"));

            // Enchantrix
            GetAddon("Enchantrix").Mappings.Add(new Mapping("5.1.3715|24.10.2008|http://auctioneeraddon.com/dl/|http://mirror.auctioneeraddon.com/dl/Packages3/Enchantrix-5.1.3715.zip", AddonSiteId.direct));
            GetAddon("Enchantrix").SubAddons.Add(GetAddon("!Swatter"));
            GetAddon("Enchantrix").SubAddons.Add(GetAddon("Enchantrix-Barker"));
            GetAddon("Enchantrix").SubAddons.Add(GetAddon("EnhTooltip"));
            GetAddon("Enchantrix").SubAddons.Add(GetAddon("Stubby"));

            // Informant
            GetAddon("Informant").Mappings.Add(new Mapping("5.1.3715|24.10.2008|http://auctioneeraddon.com/dl/#release|http://mirror.auctioneeraddon.com/dl/Packages3/Informant-5.1.3715.zip", AddonSiteId.direct));
            GetAddon("Informant").SubAddons.Add(GetAddon("!Swatter"));
            GetAddon("Informant").SubAddons.Add(GetAddon("EnhTooltip"));
            GetAddon("Informant").SubAddons.Add(GetAddon("Stubby"));

            // BottomScanner
            GetAddon("BtmScan").Mappings.Add(new Mapping("5.1.3715|24.10.2008|http://auctioneeraddon.com/dl/#release|http://mirror.auctioneeraddon.com/dl/Packages3/BottomScanner-5.1.3715.zip", AddonSiteId.direct));
            GetAddon("BtmScan").SubAddons.Add(GetAddon("!Swatter"));
            GetAddon("BtmScan").SubAddons.Add(GetAddon("EnhTooltip"));
            GetAddon("BtmScan").SubAddons.Add(GetAddon("Stubby"));
            #endregion

            // Depreciated
            //AddDepreciated("Vendor", "AuctionMaster");
            //AddDepreciated("Afflicted", "Afflicted2");

            // http://www.kroetenpower.de/download/waadu.txt

            // TODO: Mappings
            //Babble-3.3
            //FuBar_MailFu
            //Fubar_ProfessionsBookFu
            //FuBar_WindFuryFu
            //LannySimpleMicroMenuFu
            //Minimalist_ButtonFrame
            //Minimalist_ButtonFrameTitanPlugin
            //Scorecard
            //SpecialEvents-Aura-2.0
            //SpecialEvents-Bags-2.0
            //SpecialEvents-Equipped-2.0
            //SpecialEvents-LearnSpell-2.0
            //SpecialEvents-Loot-1.0
            //SpecialEvents-Mail-2.0
            //SpecialEvents-Mount-2.0
            //SpecialEvents-Movement-2.0
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

            //- ABInform
            //- AFKFix
            //- afktimer
            //- AlarBGHelper
            //- AlwaysLFG
            //- AMP
            //- AMPFu
            //- AnkhCooldownTimer
            //- AtlasQuest
            //- Auctionator
            //- Auditor2
            //- AVbars2
            //- AzCastBar
            //- AzCastBarOptions
            //- BGAlerts
            //- BGJoinTime
            //- BGSwitch
            //- CCTimer
            //- ChainLink
            //- ChatCopy
            //- ChatSounds
            //- Condensed_SpellBook
            //- CooldownToGo
            //- Cosplay
            //- CraftList2
            //- CRDelay
            //- Critline
            //- Cromulent
            //- CT_BuffMod
            //- DailyQuestViewer
            //- DamageMeters
            //- DetachedMiniButtons
            //- EnhancedFlightMap
            //- Escape Frustration
            //- Factionizer
            //- FB_InfoBar
            //- FB_MergeDatabase
            //- FB_OutfitDisplayFrame
            //- FB_Titan
            //- FB_TrackingFrame
            //- FishingBuddy
            //- Forte_Casting
            //- Forte_Cooldown
            //- Forte_Core
            //- Forte_Druid
            //- Forte_Healthstone
            //- Forte_Hunter
            //- Forte_Mage
            //- Forte_Priest
            //- Forte_Shard
            //- Forte_Soulstone
            //- Forte_Summon
            //- Forte_Timer
            //- Forte_Warlock
            //- FriendsWithBenefits
            //- FuBar_AnkhCooldownTimer
            //- FuBar_BadgeFu
            //- FuBar_BattlegroundFu
            //- FuBar_CAFu
            //- FuBar_CritlineDmg
            //- FuBar_CritlineHeal
            //- FuBar_CritlinePet
            //- FuBar_CTFu
            //- FuBar_DBMFu
            //- FuBar_DominosFu
            //- Fubar_EmoteFu
            //- FuBar_ExitFu
            //- FuBar_FactionItemsFu
            //- FuBar_FishingBuddyFu
            //- FuBar_GearRating
            //- FuBar_GroupCalendarFu
            //- FuBar_HBFu
            //- FuBar_HonorFuPlus
            //- FuBar_InstanceInfoFu
            //- FuBar_IOPFu
            //- FuBar_MailFu
            //- FuBar_MarkOfHonorFu
            //- FuBar_MiniPerfsFu
            //- FuBar_MoneyDetailFu
            //- FuBar_OutfitterFu
            //- FuBar_PointMan
            //- FuBar_RecipeRadarFu
            //- FuBar_RegenFu
            //- FuBar_ReloadUI
            //- FuBar_RestFu
            //- FuBar_RoutesFu
            //- FuBar_uClock
            //- FuBar_VampwatchFu
            //- GatherSage
            //- GemHelper
            //- GemQuota
            //- GFW_AdSpace
            //- GFW_ReagentCost
            //- GoingPrice
            //- GoingPrice_Allakhazam
            //- HandyNotes_Directions
            //- HandyNotes_Stables
            //- HandyNotes_Trainers
            //- HandyNotes_Vendors
            //- Informant
            //- InnerFireWatcher
            //- InventoryOnPar
            //- KillingBlow
            //- Lootcounter
            //- Lootvalue
            //- MageAnnounce
            //- MageFever
            //- MarkBar
            //- MetaMap
            //- MetaMapBKP
            //- MetaMapBLT
            //- MetaMapBWP
            //- MetaMapCVT
            //- MetaMapEXP
            //- MetaMapFWM
            //- MetaMapHLP
            //- MetaMapNBK
            //- MetaMapQST
            //- MetaMapTRK
            //- MetaMapWKB
            //- MobInfo2
            //- myFriends
            //- Nauticus
            //- Perl_ArcaneBar
            //- Perl_CombatDisplay
            //- Perl_Config
            //- Perl_Config_Options
            //- Perl_Focus
            //- Perl_Party
            //- Perl_Party_Pet
            //- Perl_Party_Target
            //- Perl_Player
            //- Perl_Player_Pet
            //- Perl_Target
            //- Perl_Target_Target
            //- PriceEach
            //- Quiknote
            //- RecipeBook
            //- RecipeRadar
            //- SCT_Cooldowns
            //- ShamanFriend
            //- ShieldLeft
            //- SocketAssistant
            //- Stubby
            //- TargetRange
            //- TradeJunkie
            //- UnitPrice
            //- Valuation
            //- VisualHeal
            //- WarsongFlags
            //- WatchTower

            //- AltClickToAddItem
            //- BaudAuction
            //- Cartographer_QuestInfo
            //- File13
            //- Framerate_Adjuster
            //- FriendColor
            //- FuBar_LootTypeFu
            //- MrPlow
            //- myError
            //- Nameplates
            //- NeonChat
            //- Nier
            //- PallyPower
            //- RaidIconFix
            //- StatStain
        }
    }
}
