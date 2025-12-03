namespace MiLibreria.Models
{
    public class SeriePastel
    {
        /*name: 'Chrome',
               y: 74.77,
               sliced: true,
               selected: true*/

        public string name {  get; set; }
        public double y {  get; set; }
        public bool sliced { get; set; }
        public bool selected {  get; set; }

       public SeriePastel() { }


        public SeriePastel(string name,double y,bool sliced=false, bool selected = false) {
        this.name = name;
            this.y = y;
            this .sliced = sliced;  
            this.selected = selected;
        
        }

        public List<SeriePastel> GetDataDumy()
        {
            List<SeriePastel> lista = new List<SeriePastel>();
            lista.Add( new SeriePastel() );
            return lista;
        }

    }
}
