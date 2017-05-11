﻿//using CustomElementHandler;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley.Objects;
using SFarmer = StardewValley.Farmer;

namespace SleepyEye
{
    public class TentTool : Tool//, ISaveElement
    {
        private SFarmer user;
        private DateTime? startedUsing = null;

        public TentTool()
        {
            name = "Tent";
            description = loadDescription();

            numAttachmentSlots = 0;
            indexOfMenuItemView = 0;
            Stack = 1;
        }

        public override int salePrice()
        {
            return 5000;
        }

        public override bool beginUsing(GameLocation location, int x, int y, SFarmer who)
        {
            user = who;
            startedUsing = DateTime.Now;
            who.canMove = false;
            return true;
        }

        public override bool onRelease(GameLocation location, int x, int y, SFarmer who)
        {
            if (startedUsing == null)
                return true;

            TimeSpan useTime = DateTime.Now - (DateTime)startedUsing;
            if ( useTime > TimeSpan.FromSeconds( 7 ) )
            {
                Mod.instance.saveSleepLocation = true;
                Game1.NewDay(0);
            }
            
            startedUsing = null;
            who.canMove = true;

            return true;
        }

        public override void tickUpdate(GameTime time, SFarmer who)
        {
            if ( startedUsing == null )
                return;
            TimeSpan useTime = DateTime.Now - (DateTime)startedUsing;

            if (who.facingDirection == Game1.up)
                ((FarmerSprite)who.sprite).animate(112, time);
            else if (who.facingDirection == Game1.right)
                ((FarmerSprite)who.sprite).animate(104, time);
            else if (who.facingDirection == Game1.down)
                ((FarmerSprite)who.sprite).animate(96, time);
            else if (who.facingDirection == Game1.left)
                ((FarmerSprite)who.sprite).animate(120, time);
        }

        public override void drawInMenu(SpriteBatch b, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
        {
            b.Draw(Mod.instance.Helper.Content.Load<Texture2D>("Maps/" + Game1.currentSeason + "_outdoorsTileSheet", ContentSource.GameContent), new Vector2( location.X + Game1.tileSize / 2, location.Y + Game1.tileSize / 2 ), new Rectangle(224, 96, 48, 80), Color.White, 0, new Vector2( 24, 40 ), scaleSize * 0.8f, SpriteEffects.None, 0);
        }

        public override void draw(SpriteBatch b)
        {
            CurrentParentTileIndex = indexOfMenuItemView = -999;
            if (startedUsing == null)
                return;
            
            TimeSpan useTime = DateTime.Now - (DateTime)startedUsing;

            //if ( useTime > TimeSpan.FromSeconds( 7 ) )
            {
                Color col = Color.White * (0.2f + (float)useTime.TotalSeconds / 7f * 0.6f);
                if (useTime > TimeSpan.FromSeconds(7))
                    col = Color.White;

                Vector2 pos = Game1.GlobalToLocal(Game1.player.getStandingPosition());
                pos.Y -= Game1.tileSize * 2;
                b.Draw(Mod.instance.Helper.Content.Load<Texture2D>("Maps/" + Game1.currentSeason + "_outdoorsTileSheet", ContentSource.GameContent), pos, new Rectangle(224, 96 + 80 - 16, 48, 16), col, 0, new Vector2(24, 40 - 80 + 16), 4, SpriteEffects.None, 0);
                b.Draw(Mod.instance.Helper.Content.Load<Texture2D>("Maps/" + Game1.currentSeason + "_outdoorsTileSheet", ContentSource.GameContent), pos, new Rectangle(224, 96, 48, 80 - 16), col, 0, new Vector2(24, 40), 4, SpriteEffects.None, 0.999999f);
            }
        }

        protected override string loadDescription()
        {
            return "Sleep here. Sleep there. Sleep everywhere!";
        }

        protected override string loadDisplayName()
        {
            return "Tent";
        }
    }
}
