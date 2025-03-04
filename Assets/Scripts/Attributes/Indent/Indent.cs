using UnityEngine;

namespace Attributes
{
    public class Indent : PropertyAttribute
    {
        #region Variables & Properties

        public int indentLevel;

        #endregion

        public Indent(int indentLevel) 
        {
            this.indentLevel = indentLevel;
        }
    }
}
