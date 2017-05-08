using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{        

///// <summary>
/// This is a game component that implements IUpdateable.
/// </summary>
public class SectionMoitiéMur : SectionDeNiveau
    {
        TerrainDeBase Obstacle { get; set; }
        public override float LongueurNiveau
        {
            get { return (ListeTerrains.Count()) * HitBoxBase.Z; }
        }

        public SectionMoitiéMur(Game jeu, Vector3 positionInitiale, int indexTableau)
            : base(jeu, positionInitiale, indexTableau)
        { }

    protected override void AjouterAuComponents()
    {
            base.AjouterAuComponents();
            Jeu.Components.Add(Obstacle);
            ObjetCollisionables.Add(Obstacle);
 }

    protected override void CréerSection()
    {
            Obstacle =new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN/2, Vector3.Zero, new Vector3(PositionInitiale.X + HitBoxBase.X /4f, PositionInitiale.Y + HitBoxBase.Y/2f , PositionInitiale.Z - HitBoxBase.Z), INTERVAL_MAJ, "stefpath");

            base.CréerSection();
     

    }

    /// <summary>
    /// Allows the game component to perform any initialization it needs to before starting
    /// to run.  This is where it can query for any required services and load content.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

    }

    /// <summary>
    /// Allows the game component to update itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public override void Update(GameTime gameTime)
    {
        // TODO: Add your update code here

        base.Update(gameTime);
    }

}
}