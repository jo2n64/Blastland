using System.Drawing;
using GXPEngine;
class HUD : Canvas
{
    private Player player;
    public HUD(Player player) : base(128, 128)
    {
        this.player = player;
    }

    private void Update() {
        graphics.Clear(Color.Empty);
        graphics.DrawString("Score: " + player.Score, SystemFonts.DefaultFont, Brushes.White,  0,0);
        graphics.DrawString("Weapon used: " + player.WeaponName, SystemFonts.DefaultFont, Brushes.White, 0, 16);
        graphics.DrawString("Lives: " + player.Lives, SystemFonts.DefaultFont, Brushes.White, 0, 32);
        graphics.DrawString("Fuel left " + player.Fuel, SystemFonts.DefaultFont, Brushes.White, 0, 48);
    }
}
