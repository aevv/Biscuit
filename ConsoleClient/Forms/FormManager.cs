using ConsoleClient.Forms.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Forms
{
    /// <summary>
    /// Class to control creation and deleting of win forms, allowing persistance and reference sharing easily.
    /// </summary>
    class FormManager
    {
        private readonly Dictionary<Type, List<BaseForm>> _forms;
        public Game Game { get; set; }

        public FormManager()
        {
            _forms = new Dictionary<Type, List<BaseForm>>();
        }

        public bool HasForm<T>() where T : BaseForm
        {
            return _forms.ContainsKey(typeof (T));
        }

        /// <summary>
        /// Try to open a form of the provided type if there is not too many open already.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T OpenForm<T>() where T : BaseForm
        {
            if (_forms.ContainsKey(typeof(T)))
            {
                var amount = typeof(T).GetCustomAttributes(false).OfType<OpenWindowsAttribute>().First().Amount;
                var forms = _forms[typeof(T)];

                if (forms.Count >= amount)
                {
                    // Throw?
                    return null;
                }

                var form = CreateForm<T>();
                forms.Add(form);
                return form;
            }
            else
            {
                var form = CreateForm<T>();
                _forms.Add(typeof(T), new List<BaseForm> { form });
                return form;
            }
        }

        // Needs a change to have some kind of criteria to pull from, perhaps a search func
        // For now use a string and .Contains
        /// <summary>
        /// Get a form if there are any created of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public T GetForm<T>(string criteria = "") where T : BaseForm
        {
            if (!_forms.ContainsKey(typeof (T))) return null;
            if (!string.IsNullOrEmpty(criteria))
            {
                return (T) _forms[typeof (T)].FirstOrDefault(f => f.Criteria.Contains(criteria));
            }

            return (T)_forms[typeof (T)].FirstOrDefault();
        }

        /// <summary>
        /// Get a form of the given type, or if there are none, attempt to create one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public T OpenOrGetForm<T>(string criteria = "") where T : BaseForm
        {
            return GetForm<T>() ?? OpenForm<T>();
        }

        public List<BaseForm> GetForms<T>() where T : BaseForm
        {
            var x = _forms[typeof(T)];
            return x;
        }

        public List<BaseForm> OpenOrGetForms<T>() where T : BaseForm
        {
            if (_forms.ContainsKey(typeof (T)))
            {
                return _forms[typeof (T)];
            }

            var form = CreateForm<T>();
            _forms.Add(typeof(T), new List<BaseForm> { form });

            return _forms[typeof(T)];
        }

        private T CreateForm<T>() where T : BaseForm
        {
            var form = (T)Activator.CreateInstance(typeof(T), Game);
            return form;
        }

        /// <summary>
        /// Remove a form from the manager.
        /// </summary>
        /// <param name="form">The form to remove.</param>
        public void DisposeForm(BaseForm form)
        {
            var type = form.GetType();

            if (!_forms[type].Contains(form)) return;
            _forms[type].Remove(form);
            if (_forms[type].Count == 0)
                _forms.Remove(type);
        }
    }
}
