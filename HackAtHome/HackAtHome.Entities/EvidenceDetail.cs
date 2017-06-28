using Android.Graphics;

namespace HackAtHome.Entities
{
    public class EvidenceDetail
    {
        public string Description { get; set; }

        // URL de la imagen de la evidencia.
        public string Url { get; set; }

        public Bitmap Bitmap { get; set; }

        public string BitmapString { get; set; }
    }
}