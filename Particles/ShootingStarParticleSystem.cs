using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceArcade.Particles
{
    public class ShootingStarParticleSystem : ParticleSystem
    {
        Rectangle source;

        public ShootingStarParticleSystem(Game game, Rectangle source) : base(game, 500)
        {
            this.source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "particle";
            minNumParticles = 1;
            maxNumParticles = 2;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where, Vector2.UnitY * 60, Vector2.Zero, Color.White, scale: RandomHelper.NextFloat(0.1f, 0.4f), lifetime: 9);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            AddParticles(source);
        }
    }
}
