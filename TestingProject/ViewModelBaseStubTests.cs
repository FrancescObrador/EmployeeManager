using HumanResourcesManager.Model;
using HumanResourcesManager.ViewModel;

namespace TestingProject
{
    [TestClass]
    public sealed class ViewModelBaseStubTests
    {
        private IViewModel<Employee> _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new ViewModelBaseStub<Employee>();
        }

        [TestMethod]
        public void Create_ShouldAddNewItemToItemsCollection()
        {
            var newEmployee = new Employee { id = 1, first_name = "John Doe" };

            _viewModel.Create(newEmployee);

            Assert.AreEqual(1, _viewModel.Items.Count);
            Assert.AreEqual(newEmployee, _viewModel.SelectedItem);
        }

        [TestMethod]
        public void Delete_ShouldRemoveSelectedItemFromItemsCollection()
        {
            var employee = new Employee { id = 1, first_name = "Jane Doe" };
            _viewModel.Create(employee);

            _viewModel.Delete();

            Assert.AreEqual(0, _viewModel.Items.Count);
            Assert.IsNull(_viewModel.SelectedItem);
        }

        [TestMethod]
        public void Delete_WhenNoSelectedItem_ShouldNotThrowException()
        {
            _viewModel.SelectedItem = null;

            _viewModel.Delete();
            Assert.AreEqual(0, _viewModel.Items.Count);
        }

        [TestMethod]
        public void Load_ShouldAddItemToItemsCollection()
        {
            _viewModel.Load();

            Assert.AreEqual(1, _viewModel.Items.Count);
        }

        [TestMethod]
        public void Update_ShouldTriggerPropertyChangedEvent()
        {
            var employee = new Employee { id = 1, first_name = "John Doe" };
            _viewModel.Create(employee);
            bool eventRaised = false;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_viewModel.Items))
                {
                    eventRaised = true;
                }
            };

            _viewModel.Update(employee);

            Assert.IsTrue(eventRaised, "PropertyChanged event was not raised for Items during Update.");
        }

        [TestMethod]
        public void Update_ShouldNotChangeItemCount()
        {
            var employee = new Employee { id = 1, first_name = "Jane Doe" };
            _viewModel.Create(employee);

            _viewModel.Update(employee);

            Assert.AreEqual(1, _viewModel.Items.Count);
            Assert.AreEqual(employee, _viewModel.SelectedItem);
        }
    }
}
