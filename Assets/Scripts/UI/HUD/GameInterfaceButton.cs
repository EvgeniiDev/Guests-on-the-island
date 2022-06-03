using Assets.Scripts.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.UI
{
    public class GameInterfaceButton : MonoBehaviour
    {
        public GameObject controllElements;
        public GameObject shop;
        public LayerMask whatCanBeClickedOn;

        void Update()
        {
            if (Resources.SelectedUnits.Count > 0 &&
                !(controllElements.transform.position.x <= Input.mousePosition.x &&
                controllElements.transform.position.x + 500 >= Input.mousePosition.x &&
                controllElements.transform.position.y >= Input.mousePosition.y) &&
                !(shop.transform.position.x <= Input.mousePosition.x &&
                shop.transform.position.y >= Input.mousePosition.y && shop.active))
            {
                if (Input.GetMouseButtonDown(0))
                {

                    var units = Resources.SelectedUnits.Where(x => x != null)
                                                .Select(obj => obj.GetComponent<NavMeshAgent>());

                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo, 10000))
                    {
                        foreach (var agent in units)
                        {
                            agent.ResetPath();
                            agent.SetDestination(hitInfo.point);
                            var unit = agent.GetComponent<Unit>();
                            unit.IsWalking = true;
                            unit.status = Types.Status.Chill;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                var units = Resources.SelectedUnits.Where(x => x != null)
                                            .Select(obj => obj.GetComponent<Unit>());
                foreach (var unit in units)
                {
                    unit.AttackNotFriends();
                }
            }

        }

        public void Attack()
        {
            var aims = SelectAims().ToList();

            foreach (var unit in aims)
            {
                if (unit != null)
                {
                    var nma = unit.GetComponent<NavMeshAgent>();
                    if (nma != null)
                        nma.ResetPath();
                    unit.status = Types.Status.Fighting;
                }
            }
        }

        public void Stay()
        {
            var aims = SelectAims().ToList();

            foreach (var unit in aims)
            {
                if (unit != null)
                {
                    var nma = unit.GetComponent<NavMeshAgent>();
                    if (nma != null)
                        nma.ResetPath();

                    unit.status = Types.Status.Chill;
                }
            }
        }

        public void Work()
        {
            var aims = SelectAims().ToList();

            foreach (var unit in aims)
            {
                if (unit != null)
                {
                    var nma = unit.GetComponent<NavMeshAgent>();
                    if (nma.pathEndPosition != null)
                        nma.ResetPath();

                    unit.status = Types.Status.Working;
                }
            }
        }

        private IEnumerable<Unit> SelectAims()
        {
            var friendlyUnits = Resources.SelectedUnits.Where(x => x != null)
                                     .Select(obj => (Unit)obj.GetComponent<FriendlyUnit>());
            var mainHero = Resources.SelectedUnits.Where(x => x != null)
                                             .Select(obj => (Unit)obj.GetComponent<MainCharacterUnit>());

            return friendlyUnits.Union(mainHero)
                                .Where(x=> x!=null);
        }
    }
}
