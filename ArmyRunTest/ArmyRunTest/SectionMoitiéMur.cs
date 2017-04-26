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
        const int NB_REPOS = 3;
    List<TerrainDeBase> ListeTerrains { get; set; }
        TerrainDeBase Obstacle { get; set; }
        public override float LongueurNiveau
        {
            get { return (ListeTerrains.Count()) * HitBoxBase.Z; }
        }

        public SectionMoitiéMur(Game jeu, Vector3 positionInitiale, int indexTableau)
            : base(jeu, positionInitiale, indexTableau)
        {
        ListeTerrains = new List<TerrainDeBase>();
        CréerSection();
        AjouterAuComponents();
    }

    private void AjouterAuComponents()
    {
        foreach (TerrainDeBase a in ListeTerrains)
        {
            Jeu.Components.Add(a);
            ObjetCollisionables.Add(a);
        }
            Jeu.Components.Add(Obstacle);
            ObjetCollisionables.Add(Obstacle);
        }

    private void CréerSection()
    {
            Obstacle =new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN/2, Vector3.Zero, new Vector3(PositionInitiale.X + HitBoxBase.X /4f, PositionInitiale.Y + HitBoxBase.Y/2f , PositionInitiale.Z - HitBoxBase.Z), INTERVAL_MAJ, "stefpath");

            for (int i = 0; i < NB_REPOS; ++i)
            {
                ListeTerrains.Add(new TerrainDeBase(Jeu, HOMOTHÉTIE_INITIALE_TERRAIN, Vector3.Zero, new Vector3(PositionInitiale.X, PositionInitiale.Y, PositionInitiale.Z - HitBoxBase.Z * i), INTERVAL_MAJ, "stefpath"));
            }
     

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