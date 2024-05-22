using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Quiz
    {
        public string Title { get; set; }
        public int ID { get; set; }
        public List<Question> questions { get; set; }
    }
}
