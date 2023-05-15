using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarryNightExample
{
    internal class Star
    {
        //star attributes
        private float _x, _y;       //position
        private float _angle;       //start angle
        private float _rotation;    //rotation amount per step
        private float _scale;       //sprite scale factor
        private int _lifeTime;      //how long will the star remain on the screen
        private Color _color;       //love colour
        private float _alpha;       //transparency

        private Texture2D _starSprite;      //star sprite image will be stored here


        //argumented constructor to set up custom stars
        public Star(float x, float y, float angle, float rotation, float scale, int lifeTime, Color color, float alpha, Texture2D starSprite)
        {
            _x = x;     
            _y = y;
            _angle = angle;
            _rotation = rotation;
            _scale = scale;
            _lifeTime = lifeTime;
            _color = color;
            _alpha = alpha;
            _starSprite = starSprite;
        }

        //this will happen to the star every update in the "game"
        public void Update()
        {
            _angle += _rotation;    //change the angle by the rotation amount
            _lifeTime--;            //reduce our lifetime by one step
        }

        //simple expiry when the star's lifetime variable heads below zero
        //returns try when the lifetime variable is below 0
        public bool HasExpired() { return _lifeTime <= 0; }

        //this draws the star sprite according to the state variables
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //this is the monster draw that handles rotation, scaling, and more
            spriteBatch.Draw(_starSprite, new Vector2(_x,_y), null, _color * _alpha, _angle, new Vector2(_starSprite.Width/2, _starSprite.Height/2), new Vector2(_scale, _scale), SpriteEffects.None, 0);
            spriteBatch.End();
        }

    }
}
