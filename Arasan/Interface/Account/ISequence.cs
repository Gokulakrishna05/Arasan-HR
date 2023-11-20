using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface 
{
    public interface ISequence
    {
        IEnumerable<Sequence>  GetAllSequence();
        string SequenceCRUD(Sequence Cy);
        DataTable GetSequence(string id);
        string StatusChange(string tag, int id);
        DataTable GetAllSeq();
    }
}
