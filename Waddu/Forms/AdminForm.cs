using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Waddu.AddonSites;
using Waddu.BusinessObjects;
using Waddu.Types;
using Waddu.Classes;

namespace Waddu.Forms
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();

            cbSite.DataSource = Enum.GetValues(typeof(AddonSiteId));
        }
        //ld50_ap, ld50_armor, lexan, lfg-settings-saver, lib-st, libalts, libavion-1-0, libavion-2-0, libavionaddin-1-0, libavionoptions-2-0, libbars-1-0, libbetterblizzoptions-1-0, libboneutils-1-0, libcompress, libcooldown-1-0, libcooldown-item-2-0, libcooldown-spell-2-0, libdbicon-1-0, libdebuglog-1-0, libdogtag-2-0, libdruidmana-1-0, libfilter-1-0, libfriends-1-0, libguidregistry-0-1, libguild-1-0, libinventory-2-1, libkeybound-1-0, libkeyboundextra-1-0, liblogger-1-0, liblordfarlander-2-0, mindreader, libmmbutton-1-0, libmobhealth-4-0, libquickhealth-1-0, libquickhealth-2-0, libquixote-2-0, librescomm-1-0, libroman-1-0, libsharedmedia-2-0, libsimpleframe-1-0, libspecialevents-aura-3-0, lib-spell, libstickyframes-1-0, libstickyframes-2-0, libstub, libtalentquery-1-0, libxspellid, lifepercent, light-queue, lil-sparkys-workshop, lil-sparkys-lab-partner, litmus, little-black-book, little-trouble, lock-smith, lod-what, loggerhead, loot-db, loot-sink, loot-to-guild, lootwhore, loudmouth, lucky-charms, lsrm, lug-md, lurker-position-display, lute, lzprofiler, macon, macro-bank, magealert, mage-announce, magicrunes, magic-dkp_client, magic-marker, magic-marker_data, magic-targets, magnet, mail-attachment-count, mail-info, manager, mana-meter2, manaperc, manasurge, manikin, manufac, manufac-snoop, mark-my-words, mark-my-target, master-loot, max-stats, md5lib, m-dark-pact, medley, meeuuuh, melee-buffs, melee-frame, memento-mori, menagerie, mend-watch, message-event, mhloot-switcher, m-hud, m-hud_str8, m-hud_threat, mini-chat, minimap-coords, minimap-range, minimap-target, mini-pet, mini-type, mis-information, mobile-vault, mobnotes, mo-buffs, modality, mode7, mode7test, money-trail, monkey-pirate, monkey-quest-helper, mood-music, moolah, morg-bid, morg-bid2, morg-dkp, morg-dkp2, mounted, mountiful, mountlib, mouse-look, mrhat, mr-plow, multi-tips, my-bindings2, nabu-five-sec, name-tag-toggle, nanoplayed, nanotalk, nauticus, nbuff-bars, nchide, ndrag-it, ndurability, necronomicon2, ner-frames, next-step, nielas-aran, nier, ni-hao, ninja-picker, ninja-points, no-nether, no-see-no-interrupt, no-weapons, n-raid-tracker, nrt, nrt_epgp, nshakedown, nudge, nurfed_auto-bar-hook, nurfed_button-facade, nutcounter, nvp, oblivious-dkp, ob-raid-assist, omnibus, omniscience, one-bag, one-bank, one-hit-wonder, one-ring, one-storage, one-view, open-all, open-bank-bags, o-ra2_pv-pdetection, orlic, o-uf_ammo, o-uf_banzai, o-uf_castbar, o-uf_combat-feedback, o-uf_debuff-highlight, o-uf_heal-comm, ouf_holysmurf, o-uf_incoming-heals, o-uf_mouseover, o-uf_power-spark, o-uf_quick-health, o-uf_rabbit, ouf_sb, ouf_smurf, package-tooltip, paintchipslib, pally-power-info, paparazzi, paranoia_epa, party-to-go, pass-loot, pawn-mgr, p-debuff-list, pendulum, percent, perfect-targets, pet-bar, phat-loot, phlayoutlib, picobuttons, pitbull_brutallus, pit-bull_full-bar, pitbull_kalecgos, pit-bull_lifebloom-bar, pit-bull_prat, pitbull_profile-switcher, player-menu, player-notes, p-mutilate, poison-menu, poppins, postal, postman, powerbar, power-display, prat_armory-link, prat_signon, preferred-items, prescription, prescription_pickup, p-resurrect, price-per, price-tag, private-chat, proccer, procodile, proculas, protractor, proximitylib, pt3bar, pt3bar_button-facade, pvptimer, queeg, quest-announcer, quest-collector, quest-long-list, quest-translator2, quickenchant, quiet_npc, quiet-cast, quixote-should-shut-up, quote-no-more, raid-agent, raid-cooldowns, raid-cooldowns_display, raid-gnome, raid-group-number, raid-helper, raidinfo, raid-invite-helper, raid-marshal, raid-spy, raidtargets, ka_raid-tracker, rainbow-enchants, ram-racing-records, rndtitle, rapid-cast, ratings, rat-killer, rawrkitty, r-bgrez-timer, rbm, rbm_ability-watch, rbm_badge-watch, rbm_black-temple, rbm_capping, rbm_config, rbm_drum-watch, rbm_gruuls, rbm_hunters-mark, rbm_hyjal, rbm_incubator, rbm_sunwell, rbm_totem-watch, rbm_trap-watch, rbm_zul-aman, rc4lib, ready-check-announcer, recap, recount2, recount-death-track, recountthreat, refill-remind, relegation, relegation-w, relog, repairbroker, rep-tracker, reputation, rep-watch, rtimer, reverse-engineering, rico-mini-map, rollcall-1-0, rollmaster, roots-bidder, roots-kings, rphelper2, rplanguage, rs_class-colors, ruin-boss-tactics, ruin-boss-tactics_ssc, ruin-boss-tactics_tk, rummage, sdkp, sacred-buff, sacred-buff_mounts, salvation-remover, salvation-veto, salv-begone, sanity2, sanity-bags, sanity-item-cache, satrina-buff-frame, scaffold, school, scorchio, score-sort, scrolling-combat-text, scrub, scrub-fork, sct-damage, sct_cooldowns, sct_spell-ready, seal-bar, sealit, seasy-rotation, seeing-red, sell-o-matic, sell-junk, serenity, serious-buff-timers, shadow-friend, shamanwarning, shaman-buff-bars, shammy-spy, shared-media-blizzard, shepherd, sheppard, shopping-list, shut-up, silver-dragon, simpleaurafilter, simple-bar, simple-boss-whisperer, simple-chat-mods, simple-drum-announce, simple-md, simple-price-each, simple-soulstone, simple-soulstone_fu, simple-tradeskill, simple-unit-frames, sinklib, skg, slice-watcher, smart-fitter, smart-queue, smart-res, smarty-cat, smarty-cat-mana-bar, smooth-quest, sniff, soar-1-0, socialist, specialeventsembed, spell-binder, spelleventslib, spell-reminder, spell-watch, spew, splut, spygnome, squeenix, sra, s-raid-frames_quick-health, sraidframesindicators, stable, stack-watch, stance-sets-fu, sbars, stat-block_combatlog, stat-block_coords, stat-block_folks, stat-block_melee-stats, stat-block_quests, stat-block_ranged-stats, stat-block_tank-stats, stat-block_time-in-fight, stat-block_xp, statframelib, stat-stain, stealyourcarbon, steroids, summon-bot, summoning-bar, surfacelib, surfaces, surgeon-general, swap-magic, syn-camera, syn-study, t_item-info, t_rpp, t_tracking, tabard, table-inspector, tabletlib, tabletoplib, tactik-sod, taggle, taint-checker, talented_dr-damage, talented_scrub, talent-safe, talentshow-1-0, talent-whisper, talismonger-3-0, tankadin, tankalyze, tattle_lfm, tealib, tekmapenhancer, teknicolor, teknicolor_tab, teksloot, tekticles, test-alpha, text-spy, tguild-frame, thanks-for-the-hint, thesocial-1-0, threat-2-0, three-armed-bandit, tick-to, tier-tooltip, time-to-die, tinykos, tiny-stats, tiny-tip, tinytipbasic, tinytipoptions, tinytippositioning, tinytiptargets, tiphookerlib, titan-professions, titleswapper, title-hider, tmrecruit, toolbox-1-0, tooltip-exchange, tooltip-exchange_item-database, tooltip-item-info, tourguide, track-o-matique, tracking-bar, tracking-info, tracksw, trade-bar, tradeskill-info, tradeskill-info-ui, trade-stop, training-wheels, trap-bar, tuck-unit-frames, tui-raid-tracker, tunnel-vision, tweak-hub, tweak-hub_automation, tweak-hub_chat, tweak-hub_combat-logger, tweak-hub_inbox, tweak-hub_merchant, tweak-hub_minimap, tweak-hub_reputation, tweak-hub_rwfilter, twi-co, twin-cam_overlay, typing-indicator, uiquery, undercut, unicode-font, unit-action-bars, unit-action-bars2, unit-price, unsolicited, utopia, valuation, v-chat, veneration, vengance, version-checker, vibrant, view-addon-message, violation, visor2, visualheal, visual-mail, visual-themes, vital-watch, vital-watch-options, vox, voyeur, waitlist, warcraftpets, wardrobe2, waterfall-1-0, welfareepics, wf3sec, wftotem, whats, wheres-my-cow, whisp, whisper-bid, whisper-forward, whisper-loot, whiteboard-ng, who-drop, who-favorites, winded, wishlist, world-trade-center, wo-wonid, w-rlog, wrong-button, xart, xc-raid, xparky, xpbarnone, yap, yell-collect, yikes, you-were-killed-by, yss-dark-pact, yss-guild-shop, zim-clock, zim-coords, zim-greed, zoo-keeper, easydkp
        private void CreateMappings(string tag, AddonSiteId siteId)
        {
            txtOut.Text = string.Empty;

            string[] tagList = tag.Split(',');
            foreach (string itTag in tagList)
            {
                string currTag = itTag.Trim();
                try
                {
                    AddonSiteBase site = AddonSiteBase.GetSite(siteId);
                    Mapping map = new Mapping(currTag, siteId);
                    map.Addon = new Addon("tag");
                    string downLink = site.GetDownloadLink(map);
                    string archiveFilePath = WebHelper.DownloadFileToTemp(downLink);
                    List<string> cont = ArchiveHelper.GetArchiveContent(archiveFilePath);
                    List<string> folderList = new List<string>();
                    foreach (string s in cont)
                    {
                        int index = s.IndexOf("\\");
                        if (index > 0)
                        {
                            Helpers.AddIfNeeded<string>(folderList, s.Substring(0, index));
                        }
                    }
                    string main = string.Empty;
                    for (int i = 0; i < folderList.Count; i++)
                    {
                        string s = folderList[i];
                        if (i == 0)
                        {
                            txtOut.Text += string.Format(@"GetAddon(""{0}"").Mappings.Add(new Mapping(""{1}"", AddonSiteId.{2}));{3}", s, currTag, siteId, Environment.NewLine);
                            main = s;
                        }
                        else
                        {
                            txtOut.Text += string.Format(@"GetAddon(""{0}"").SubAddons.Add(GetAddon(""{1}""));{2}", main, s, Environment.NewLine);
                        }
                    }
                }
                catch
                {
                    txtOut.Text += string.Format(@"Tag ""{0}"" failed{1}", currTag, Environment.NewLine);
                }
                txtOut.Text += Environment.NewLine;
                Application.DoEvents();
                //System.Threading.Thread.Sleep(1000);
            }
        }

        private void btnCreateMapping_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text;
            Match m = Regex.Match(url, "http://wow.curse.com/downloads/wow-addons/details/(.*).aspx");
            if (m.Success)
            {
            }
        }

        private void btnCreateFromTag_Click(object sender, EventArgs e)
        {
            string tag = txtUrl.Text;
            CreateMappings(tag, GetSite());
        }

        private string GetUrl()
        {
            AddonSiteId site = GetSite();
            if (site == AddonSiteId.curse)
            {
                return "http://wow.curse.com/downloads/wow-addons/details/(.*).aspx";
            }
            return string.Empty;
        }

        private AddonSiteId GetSite()
        {
            return (AddonSiteId)cbSite.SelectedItem;
        }
    }
}
