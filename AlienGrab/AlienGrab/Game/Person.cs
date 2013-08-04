using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AlienGrab
{
    class Person : Base3DObject
    {
        protected float leftArmRotate = 0.0f;
        protected float leftArmRotateSpeed = 0.1f;
        protected float rightArmRotate = 0.0f;
        protected float rightArmRotateSpeed = 0.1f;

        protected float jumpSpeed = 1.0f;
        protected int jumpMaxHeight = 10;
        protected int jumpHeight = 0;
        protected Matrix[] meshWorldMatrices;
        public Person(Game game, String assetName, LightSource _light)
            : base(game, assetName, _light)
        {
            meshWorldMatrices = new Matrix[6];
            meshWorldMatrices[0] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            meshWorldMatrices[1] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            meshWorldMatrices[2] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            meshWorldMatrices[3] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            //left arm
            meshWorldMatrices[4] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            //right arm
            meshWorldMatrices[5] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        }

        public override void Initialize()
        {
            Scale = new Vector3(0.25f, 0.25f, 0.25f);
            Rotation = new Vector3(0.0f, 80.0f, 0.0f);
            OverrideFirstCollision = true;
            elementsWorldMatrix = meshWorldMatrices;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            leftArmRotate += leftArmRotateSpeed;
            if (leftArmRotate > 0.1f || leftArmRotate < -1.0f)
            {
                leftArmRotateSpeed *= -1;
            }
            rightArmRotate += rightArmRotateSpeed;
            if (rightArmRotate > 0.9f || rightArmRotate < -0.1f)
            {
                rightArmRotateSpeed *= -1;
            }

            if (jumpHeight > jumpMaxHeight)
            {
                jumpSpeed *= -1;
                jumpHeight = 0;
            }
            Position.Y += jumpSpeed;
            jumpHeight++;
            //left arm
            meshWorldMatrices[4] = Matrix.CreateTranslation(new Vector3(0.0f, -75.0f, 0.0f)) * Matrix.CreateRotationZ(leftArmRotate) * Matrix.CreateTranslation(new Vector3(0.0f, 75.0f, 0.0f));
            //right arm
            meshWorldMatrices[5] = Matrix.CreateTranslation(new Vector3(0.0f, -75.0f, 0.0f)) * Matrix.CreateRotationZ(rightArmRotate) * Matrix.CreateTranslation(new Vector3(0.0f, 75.0f, 0.0f));
            base.Update(gameTime);
        }
    }
}
